using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class RecurringTransactionWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RecurringTransactionWorker> _logger;

    public RecurringTransactionWorker(IServiceProvider serviceProvider, ILogger<RecurringTransactionWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Recurring Transaction Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessDueTransactionsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred processing recurring transactions.");
            }

            // Run check every 1 hour
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task ProcessDueTransactionsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();

        var now = DateTime.UtcNow;

        var dueRecurring = await dbContext.RecurringTransactions
            .Where(rt => rt.NextDueDate <= now)
            .ToListAsync(stoppingToken);

        if (!dueRecurring.Any())
            return;

        int processedCount = 0;

        foreach (var recurring in dueRecurring)
        {
            // 1. Create the actual transaction record
            var newTx = new Transaction
            {
                Description = recurring.Description,
                Amount = recurring.Amount,
                Type = recurring.Type,
                Date = recurring.NextDueDate, // Log it exactly when it was due
                CategoryId = recurring.CategoryId,
                AccountId = recurring.AccountId,
                ToAccountId = recurring.ToAccountId
            };

            dbContext.Transactions.Add(newTx);

            // 2. Adjust account balances
            var account = await dbContext.Accounts.FindAsync(new object[] { recurring.AccountId }, stoppingToken);
            if (account != null)
            {
                // Account balances are calculated dynamically from Transactions in GetTotalBalanceAsync/GetAccountBalanceAsync
                // We don't need to manually update an 'Account.Balance' field here because the system calculates it based off the Transactions table.
                // However, if we change the system later, this is where we'd do it.
            }

            // 3. Update the NextDueDate for the recurring template
            recurring.NextDueDate = CalculateNextDueDate(recurring.NextDueDate, recurring.Frequency);
            
            processedCount++;
        }

        await dbContext.SaveChangesAsync(stoppingToken);
        _logger.LogInformation($"Processed {processedCount} recurring transactions.");
    }

    private DateTime CalculateNextDueDate(DateTime currentDate, TransactionFrequency frequency)
    {
        return frequency switch
        {
            TransactionFrequency.Daily => currentDate.AddDays(1),
            TransactionFrequency.Weekly => currentDate.AddDays(7),
            TransactionFrequency.Monthly => currentDate.AddMonths(1),
            TransactionFrequency.Yearly => currentDate.AddYears(1),
            _ => currentDate.AddMonths(1)
        };
    }
}

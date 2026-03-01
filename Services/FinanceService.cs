using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class FinanceService
{
    private readonly FinanceDbContext _context;

    public FinanceService(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetTransactionsAsync(int? month = null, int? year = null)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Include(t => t.ToAccount)
            .AsQueryable();

        if (month.HasValue && year.HasValue)
        {
            var startDate = new DateTime(year.Value, month.Value, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = startDate.AddMonths(1);
            query = query.Where(t => t.Date >= startDate && t.Date < endDate);
        }

        return await query
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(decimal Income, decimal Expense)> GetMonthlyStatsAsync(int month, int year)
    {
        var startDate = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddMonths(1);

        var transactions = await _context.Transactions
            .Where(t => t.Date >= startDate && t.Date < endDate)
            .ToListAsync();

        var income = transactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);

        var expense = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

        // Transfers don't count towards income/expense stats at the global level
        return (income, expense);
    }

    public async Task<decimal> GetTotalBalanceAsync()
    {
        var initialBalances = (await _context.Accounts.Select(a => a.InitialBalance).ToListAsync()).Sum();

        var income = (await _context.Transactions
            .Where(t => t.Type == TransactionType.Income)
            .Select(t => t.Amount)
            .ToListAsync()).Sum();
        
        var expense = (await _context.Transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Select(t => t.Amount)
            .ToListAsync()).Sum();

        return initialBalances + income - expense;
    }

    public async Task<List<Account>> GetAccountsAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<decimal> GetAccountBalanceAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null) return 0;

        var income = (await _context.Transactions
            .Where(t => (t.AccountId == accountId && t.Type == TransactionType.Income) || 
                        (t.ToAccountId == accountId && t.Type == TransactionType.Transfer))
            .Select(t => t.Amount)
            .ToListAsync()).Sum();

        var expense = (await _context.Transactions
            .Where(t => (t.AccountId == accountId && t.Type == TransactionType.Expense) || 
                        (t.AccountId == accountId && t.Type == TransactionType.Transfer))
            .Select(t => t.Amount)
            .ToListAsync()).Sum();

        return account.InitialBalance + income - expense;
    }

    public async Task AddAccountAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account != null)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}

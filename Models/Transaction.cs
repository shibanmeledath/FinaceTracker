using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models;

public enum TransactionType
{
    Income,
    Expense,
    Transfer
}

public class Transaction
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

   
    public int ? CategoryId { get; set; }

    public Category? Category { get; set; }

    [Required(ErrorMessage = "Account is required")]
    public int AccountId { get; set; }
    public Account? Account { get; set; }

    public int? ToAccountId { get; set; }
    public Account? ToAccount { get; set; }

    [Required]
    public TransactionType Type { get; set; } = TransactionType.Expense;
}

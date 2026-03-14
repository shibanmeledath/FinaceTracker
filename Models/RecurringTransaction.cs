using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models;

public class RecurringTransaction
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }
    
    public TransactionFrequency Frequency { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime NextDueDate { get; set; } // The actual date the background worker looks for

    // Foreign Keys
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    [Required]
    public int AccountId { get; set; }
    public Account? Account { get; set; }

    public int? ToAccountId { get; set; }
    public Account? ToAccount { get; set; }
}

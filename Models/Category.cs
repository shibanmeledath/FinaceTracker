using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Icon is required")]
    [StringLength(10)]
    public string Icon { get; set; } = "📁";

    [Required(ErrorMessage = "Color is required")]
    [StringLength(20)]
    public string Color { get; set; } = "#cccccc";

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

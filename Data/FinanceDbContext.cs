using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using FinanceTracker.Models;

namespace FinanceTracker.Data;

public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global DateTime converter for Npgsql UTC compatibility
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
            v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc));

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        }

        // Seed some initial categories
      modelBuilder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Food", Icon = "🍔", Color = "#FF5733" },
        new Category { Id = 2, Name = "Transport", Icon = "🚗", Color = "#33FF57" },
        new Category { Id = 3, Name = "Groceries", Icon = "🛒", Color = "#16A34A" },
        new Category { Id = 4, Name = "Entertainment", Icon = "🎬", Color = "#F333FF" },
        new Category { Id = 5, Name = "Salary", Icon = "💰", Color = "#33FFFF" },
        new Category { Id = 6, Name = "Divident", Icon = "🏦", Color = "#1E3A8A" },
        new Category { Id = 7, Name = "Emi", Icon = "💳", Color = "#1E3A8A" },
        new Category { Id = 8, Name = "Fuel", Icon = "⛽", Color = "#DC2626" },
        new Category { Id = 9, Name = "Gold", Icon = "⚪", Color = "#D4AF37" },
        new Category { Id = 10, Name = "Mother", Icon = "💖", Color = "#7C3AED" },
        new Category { Id = 11, Name = "Gift", Icon = "🎁", Color = "#E11D48" },
        new Category { Id = 12, Name = "Cashback", Icon = "💸", Color = "#059669" },
        new Category { Id = 13, Name = "Gym", Icon = "💪", Color = "#2563EB" },
        new Category { Id = 14, Name = "Recharge", Icon = "📱", Color = "#0EA5E9" },
        new Category { Id = 15, Name = "SIP", Icon = "📈", Color = "#047857" },
        new Category { Id = 16, Name = "Vending machine", Icon = "🍫", Color = "#F97316" },
        new Category { Id = 17, Name = "Drinks", Icon = "☕", Color = "#7C2D12" }
    );

     modelBuilder.Entity<Account>().HasData(
        new Account { Id = 1, Name = "Salary Account", Icon = "🏦", Color = "#4A90E2", InitialBalance = 4383.67m },
        new Account { Id = 2, Name = "Spending Account", Icon = "💰", Color = "#50E3C2", InitialBalance = 55.39m },
        new Account { Id = 3, Name = "Savings Account", Icon = "💳", Color = "#D0021B", InitialBalance = 4399.01m },
        new Account { Id = 4, Name = "Investments", Icon = "💰", Color = "#3b82f6", InitialBalance = 0m }
    );
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToAccount)
            .WithMany()
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

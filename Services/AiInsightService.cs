using FinanceTracker.Models;

namespace FinanceTracker.Services;

public class AiInsightService
{
    public List<string> GenerateInsights(decimal income, decimal expense, List<CategoryBreakdown> categoryBreakdowns)
    {
        var insights = new List<string>();

        if (income == 0 && expense == 0)
        {
             insights.Add("💡 No financial activity recorded for this period. Add transactions to see insights.");
             return insights;
        }

        // 1. Overall Balance Check & Savings Rate
        if (expense > income && income > 0)
        {
            var deficit = expense - income;
            insights.Add($"⚠️ Warning: You spent ₹{deficit} more than you earned this month. Review your non-essential expenses to avoid debt.");
        }
        else if (expense > 0 && income == 0)
        {
            insights.Add($"⚠️ Alert: You had ₹{expense} in expenses but recorded 0 income. Be careful if you are dipping into savings.");
        }
        else if (income > expense)
        {
            var savings = income - expense;
            var savingsRate = (savings / income) * 100;
            if (savingsRate >= 20)
            {
                 insights.Add($"🌟 Excellent! You saved {savingsRate:F1}% of your income. Consider investing this surplus.");
            }
            else
            {
                 insights.Add($"👍 Good job staying positive! You saved {savingsRate:F1}% of your income. Try aiming for a 20% savings rate.");
            }
        }

        // 2. Category Analysis
        var expenseCategories = categoryBreakdowns.Where(c => c.Type == TransactionType.Expense && c.Amount > 0).ToList();
        
        if (expenseCategories.Any())
        {
            var topExpense = expenseCategories.OrderByDescending(c => c.Amount).First();
            var totalExpense = expenseCategories.Sum(c => c.Amount);
            var topExpensePercentage = (topExpense.Amount / totalExpense) * 100;

            insights.Add($"📊 Your largest expense was '{topExpense.CategoryName}' (₹{topExpense.Amount}), accounting for {topExpensePercentage:F1}% of your total spending.");

            if (topExpensePercentage > 40)
            {
                insights.Add($"💡 Action: Try setting a strict budget for '{topExpense.CategoryName}'. Reducing this by just 10% next month saves ₹{topExpense.Amount * 0.1m}.");
            }
            
            if (expenseCategories.Count >= 3)
            {
                 var top3Sum = expenseCategories.OrderByDescending(c => c.Amount).Take(3).Sum(c => c.Amount);
                 var top3Percentage = (top3Sum / totalExpense) * 100;
                 if (top3Percentage > 80)
                 {
                     insights.Add($"🔍 Note: {top3Percentage:F1}% of your spending is concentrated in just 3 categories. This is normal, but ensure these are necessary costs.");
                 }
            }
        }
        else if (expense == 0 && income > 0)
        {
             insights.Add("🎉 All income and no expenses! You had a completely spend-free month.");
        }

        return insights;
    }
}

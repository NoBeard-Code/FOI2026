using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Dashboard
{
    public partial class Dashboard
    {
        [Inject]
        public DashboardService DashboardService { get; set; }

        private int totalStock;
        private decimal stockValue;

        private List<(string Name, int Quantity)> topArticles = new();

        protected override async Task OnInitializedAsync()
        {
            totalStock = await DashboardService.GetTotalStockQuantityAsync();
            stockValue = await DashboardService.GetStockValueAsync();
            topArticles = await DashboardService.GetTopSellingArticlesAsync();
        }

        private int maxQuantity => topArticles.Any()
    ? topArticles.Max(x => x.Quantity)
    : 1;

        private int GetBarWidth(int value)
        {
            return (int)((double)value / maxQuantity * 100);
        }

        private Order? latestOrder;
    }
}
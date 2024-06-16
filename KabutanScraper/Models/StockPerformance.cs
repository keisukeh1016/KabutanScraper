namespace KabutanScraper;

public class StockPerformance
{
    public decimal? NetSales { get; set; }
    public decimal? OperatingProfit { get; set; }
    public decimal? OrdinaryProfit { get; set; }
    public decimal? Profit { get; set; }
    public decimal? EarningsPerShare { get; set; }

    public decimal? OperatingProfitRate
    {
        get => Rate(OperatingProfit, NetSales);
    }

    public decimal? OrdinaryProfitRate
    {
        get => Rate(OrdinaryProfit, NetSales);
    }

    public decimal? ProfitRate
    {
        get => Rate(Profit, NetSales);
    }

    private static decimal? Rate(decimal? cur, decimal? pre)
    {
        if (cur != null && cur > 0 && pre != null && pre > 0)
        {
            return cur / pre;
        }
        else
        {
            return null;
        }
    }
}

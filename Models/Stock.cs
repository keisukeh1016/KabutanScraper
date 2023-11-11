namespace KabutanScraper;

public class Stock
{
    public Stock(string code, string name, Constants.Market market, int quarter)
    {
        Code = code;
        Name = name;
        Market = market;
        Quarter = quarter;
    }

    public string Code { get; set; }
    public string Name { get; set; }
    public Constants.Market Market { get; set; }
    public int Quarter { get; set; }
    public StockPerformance QuarterPerformance { get; set; } = new StockPerformance();
    public StockPerformance QuarterPerformance1TimesBefore { get; set; } = new StockPerformance();
    public StockPerformance QuarterPerformance2TimesBefore { get; set; } = new StockPerformance();
    public StockPerformance QuarterPerformance3TimesBefore { get; set; } = new StockPerformance();
    public StockPerformance QuarterPerformance4TimesBefore { get; set; } = new StockPerformance();
    public StockPerformance YtdPerformance { get; set; } = new StockPerformance();
    public StockPerformance YtdPerformance1TimesBefore { get; set; } = new StockPerformance();
    public StockPerformance YearPerformance { get; set; } = new StockPerformance();
    public StockPerformance YearPerformance1TimesBefore { get; set; } = new StockPerformance();

    public decimal? QoqNetSales
    {
        get => Rate(QuarterPerformance.NetSales, QuarterPerformance1TimesBefore.NetSales);
    }

    public decimal? QoqOperatingProfit
    {
        get => Rate(QuarterPerformance.OperatingProfit, QuarterPerformance1TimesBefore.OperatingProfit);
    }

    public decimal? QoqOrdinaryProfit
    {
        get => Rate(QuarterPerformance.OrdinaryProfit, QuarterPerformance1TimesBefore.OrdinaryProfit);
    }

    public decimal? QoqProfit
    {
        get => Rate(QuarterPerformance.Profit, QuarterPerformance1TimesBefore.Profit);
    }

    public decimal? QoqEarningsPerShare
    {
        get => Rate(QuarterPerformance.EarningsPerShare, QuarterPerformance1TimesBefore.EarningsPerShare);
    }

    public decimal? YoyNetSales
    {
        get => Rate(QuarterPerformance.NetSales, QuarterPerformance4TimesBefore.NetSales);
    }

    public decimal? YoyOperatingProfit
    {
        get => Rate(QuarterPerformance.OperatingProfit, QuarterPerformance4TimesBefore.OperatingProfit);
    }

    public decimal? YoyOrdinaryProfit
    {
        get => Rate(QuarterPerformance.OrdinaryProfit, QuarterPerformance4TimesBefore.OrdinaryProfit);
    }

    public decimal? YoyProfit
    {
        get => Rate(QuarterPerformance.Profit, QuarterPerformance4TimesBefore.Profit);
    }

    public decimal? YoyEarningsPerShare
    {
        get => Rate(QuarterPerformance.EarningsPerShare, QuarterPerformance4TimesBefore.EarningsPerShare);
    }

    public decimal? YtdYoyNetSales
    {
        get => Rate(YtdPerformance.NetSales, YtdPerformance1TimesBefore.NetSales);
    }

    public decimal? YtdYoyOperatingProfit
    {
        get => Rate(YtdPerformance.OperatingProfit, YtdPerformance1TimesBefore.OperatingProfit);
    }

    public decimal? YtdYoyOrdinaryProfit
    {
        get => Rate(YtdPerformance.OrdinaryProfit, YtdPerformance1TimesBefore.OrdinaryProfit);
    }

    public decimal? YtdYoyProfit
    {
        get => Rate(YtdPerformance.Profit, YtdPerformance1TimesBefore.Profit);
    }

    public decimal? YtdYoyEarningsPerShare
    {
        get => Rate(YtdPerformance.EarningsPerShare, YtdPerformance1TimesBefore.EarningsPerShare);
    }

    public decimal? FiscalYoyNetSales
    {
        get => Rate(YearPerformance.NetSales, YearPerformance1TimesBefore.NetSales);
    }

    public decimal? FiscalYoyOperatingProfit
    {
        get => Rate(YearPerformance.OperatingProfit, YearPerformance1TimesBefore.OperatingProfit);
    }

    public decimal? FiscalYoyOrdinaryProfit
    {
        get => Rate(YearPerformance.OrdinaryProfit, YearPerformance1TimesBefore.OrdinaryProfit);
    }

    public decimal? FiscalYoyProfit
    {
        get => Rate(YearPerformance.Profit, YearPerformance1TimesBefore.Profit);
    }

    public decimal? FiscalYoyEarningsPerShare
    {
        get => Rate(YearPerformance.EarningsPerShare, YearPerformance1TimesBefore.EarningsPerShare);
    }

    private static decimal? Rate(decimal? cur, decimal? pre)
    {
        if (cur != null && cur > 0 && pre != null && pre > 0)
        {
            return (cur - pre) / pre;
        }
        else
        {
            return null;
        }
    }
}

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

    public decimal? QoqNetSales
    {
        get
        {
            decimal? cur = QuarterPerformance.NetSales;
            decimal? pre = QuarterPerformance1TimesBefore.NetSales;

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

    public decimal? QoqOperatingProfit
    {
        get
        {
            decimal? cur = QuarterPerformance.OperatingProfit;
            decimal? pre = QuarterPerformance1TimesBefore.OperatingProfit;

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

    public decimal? QoqOrdinaryProfit
    {
        get
        {
            decimal? cur = QuarterPerformance.OrdinaryProfit;
            decimal? pre = QuarterPerformance1TimesBefore.OrdinaryProfit;

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

    public decimal? QoqProfit
    {
        get
        {
            decimal? cur = QuarterPerformance.Profit;
            decimal? pre = QuarterPerformance1TimesBefore.Profit;

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

    public decimal? QoqEarningsPerShare
    {
        get
        {
            decimal? cur = QuarterPerformance.EarningsPerShare;
            decimal? pre = QuarterPerformance1TimesBefore.EarningsPerShare;

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

    public decimal? YoyNetSales
    {
        get
        {
            decimal? cur = QuarterPerformance.NetSales;
            decimal? pre = QuarterPerformance4TimesBefore.NetSales;

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

    public decimal? YoyOperatingProfit
    {
        get
        {
            decimal? cur = QuarterPerformance.OperatingProfit;
            decimal? pre = QuarterPerformance4TimesBefore.OperatingProfit;

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

    public decimal? YoyOrdinaryProfit
    {
        get
        {
            decimal? cur = QuarterPerformance.OrdinaryProfit;
            decimal? pre = QuarterPerformance4TimesBefore.OrdinaryProfit;

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

    public decimal? YoyProfit
    {
        get
        {
            decimal? cur = QuarterPerformance.Profit;
            decimal? pre = QuarterPerformance4TimesBefore.Profit;

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

    public decimal? YoyEarningsPerShare
    {
        get
        {
            decimal? cur = QuarterPerformance.EarningsPerShare;
            decimal? pre = QuarterPerformance4TimesBefore.EarningsPerShare;

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
}

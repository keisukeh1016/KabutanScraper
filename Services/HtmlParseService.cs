using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace KabutanScraper;

public class HtmlParseService
{
    public static async Task<Stock?> ParseHtml(string html)
    {
        var parser = new HtmlParser();
        var doc = await parser.ParseDocumentAsync(html);

        string? code = doc
            .QuerySelectorAll("#stockinfo_i1 > div.si_i1_1 > h2 > span")
            .FirstOrDefault()
            ?.Text()
            ?.Trim();
        if (code == null)
        {
            return null;
        }

        string? name = doc
            .QuerySelectorAll("#stockinfo_i1 > div.si_i1_1 > h2")
            .FirstOrDefault()
            ?.Text()
            ?.Replace(code, "")
            ?.Trim();
        if (name == null)
        {
            return null;
        }

        string? market = doc
            .QuerySelectorAll("#stockinfo_i1 > div.si_i1_1 > span")
            .FirstOrDefault()
            ?.Text()
            ?.Trim();
        if (market == null || !Constants.MarketNames().Contains(market))
        {
            return null;
        }

        string? quarter = doc
            .QuerySelectorAll("#finance_box > div.fin_quarter_t0_d.fin_quarter_result_d")
            .FirstOrDefault()
            ?.PreviousElementSibling
            ?.QuerySelectorAll("div.cap2.cap2s > img")
            ?.FirstOrDefault()
            ?.GetAttribute("src");
        if (quarter == null)
        {
            return null;
        }
        if (quarter.EndsWith("1Q.jpg"))
        {
            quarter = "1";
        }
        else if (quarter.EndsWith("2Q.jpg"))
        {
            quarter = "2";
        }
        else if (quarter.EndsWith("3Q.jpg"))
        {
            quarter = "3";
        }
        else if (quarter.EndsWith("4Q.jpg"))
        {
            quarter = "4";
        }
        else
        {
            return null;
        }

        IElement? quarterPerformance = doc
            .QuerySelectorAll("#finance_box > div.fin_quarter_t0_d.fin_quarter_result_d > table > tbody > tr > th")
            .Where(th => th.Text() == "前年同期比")
            .FirstOrDefault()
            ?.ParentElement
            ?.PreviousElementSibling;

        IElement? quarterPerformance1TimesBefore = quarterPerformance?.PreviousElementSibling;
        IElement? quarterPerformance2TimesBefore = quarterPerformance1TimesBefore?.PreviousElementSibling;
        IElement? quarterPerformance3TimesBefore = quarterPerformance2TimesBefore?.PreviousElementSibling;
        IElement? quarterPerformance4TimesBefore = quarterPerformance3TimesBefore?.PreviousElementSibling;

        Stock stock = new Stock(
            code,
            name,
            Enum.Parse<Constants.Market>(market),
            int.Parse(quarter)
        )
        {
            QuarterPerformance = ParsePerformanceHtml(quarterPerformance),
            QuarterPerformance1TimesBefore = ParsePerformanceHtml(quarterPerformance1TimesBefore),
            QuarterPerformance2TimesBefore = ParsePerformanceHtml(quarterPerformance2TimesBefore),
            QuarterPerformance3TimesBefore = ParsePerformanceHtml(quarterPerformance3TimesBefore),
            QuarterPerformance4TimesBefore = ParsePerformanceHtml(quarterPerformance4TimesBefore),
        };

        return stock;
    }

    public static async Task<List<Stock?>> ParseHtmlList(IList<string> htmlList)
    {
        List<Task<Stock?>> tasks = new List<Task<Stock?>> { };

        foreach (var html in htmlList)
        {
            Task<Stock?> task = ParseHtml(html);
            tasks.Add(task);
        }

        Stock?[] stocks = await Task.WhenAll(tasks);
        return stocks.ToList();
    }

    public static StockPerformance ParsePerformanceHtml(IElement? row)
    {
        string? NetSales = row
            ?.QuerySelectorAll("td:nth-child(2)")
            ?.FirstOrDefault()
            ?.Text()
            ?.Replace(",", "")
            ?.Replace("－", "")
            ?.Trim();

        string? OperatingProfit = row
            ?.QuerySelectorAll("td:nth-child(3)")
            ?.FirstOrDefault()
            ?.Text()
            ?.Replace(",", "")
            ?.Replace("－", "")
            ?.Trim();

        string? OrdinaryProfit = row
            ?.QuerySelectorAll("td:nth-child(4)")
            ?.FirstOrDefault()
            ?.Text()
            ?.Replace(",", "")
            ?.Replace("－", "")
            ?.Trim();

        string? Profit = row
            ?.QuerySelectorAll("td:nth-child(5)")
            ?.FirstOrDefault()
            ?.Text()
            ?.Replace(",", "")
            ?.Replace("－", "")
            ?.Trim();

        string? EarningsPerShare = row
            ?.QuerySelectorAll("td:nth-child(6)")
            ?.FirstOrDefault()
            ?.Text()
            ?.Replace(",", "")
            ?.Replace("－", "")
            ?.Trim();

        StockPerformance performance = new StockPerformance()
        {
            NetSales = string.IsNullOrEmpty(NetSales) ? null : decimal.Parse(NetSales),
            OperatingProfit = string.IsNullOrEmpty(OperatingProfit) ? null : decimal.Parse(OperatingProfit),
            OrdinaryProfit = string.IsNullOrEmpty(OrdinaryProfit) ? null : decimal.Parse(OrdinaryProfit),
            Profit = string.IsNullOrEmpty(Profit) ? null : decimal.Parse(Profit),
            EarningsPerShare = string.IsNullOrEmpty(EarningsPerShare) ? null : decimal.Parse(EarningsPerShare)
        };

        return performance;
    }
}

using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace KabutanScraper;

public static class HtmlParseService
{
    public static async Task<Stock?> ParseHtml(string html)
    {
        var parser = new HtmlParser();
        IHtmlDocument doc = await parser.ParseDocumentAsync(html);

        string? code = ParseCodeHtml(doc);
        if (code == null)
        {
            return null;
        }

        string? name = ParseNameHtml(doc, code);
        if (name == null)
        {
            return null;
        }

        string? market = ParseMarketHtml(doc);
        if (market == null || !Constants.MarketNames().Contains(market))
        {
            return null;
        }

        string? quarter = ParseQuarterHtml(doc);
        if (quarter == null)
        {
            return null;
        }

        IElement? yearPerformance = doc
            .QuerySelectorAll("#finance_box > div.fin_year_t0_d.fin_year_result_d > table > tbody > tr > th")
            .Where(th => th.Text() == "前期比")
            .FirstOrDefault()
            ?.ParentElement
            ?.PreviousElementSibling;

        IElement? yearPerformance1TimesBefore = yearPerformance?.PreviousElementSibling;
        IElement? yearPerformance2TimesBefore = yearPerformance1TimesBefore?.PreviousElementSibling;
        IElement? yearPerformance3TimesBefore = yearPerformance2TimesBefore?.PreviousElementSibling;
        IElement? yearPerformance4TimesBefore = yearPerformance3TimesBefore?.PreviousElementSibling;

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
            YearPerformance = yearPerformance == null ? new StockPerformance() : ParsePerformanceHtml(yearPerformance),
            YearPerformance1TimesBefore = yearPerformance1TimesBefore == null ? new StockPerformance() : ParsePerformanceHtml(yearPerformance1TimesBefore),
            YearPerformance2TimesBefore = yearPerformance2TimesBefore == null ? new StockPerformance() : ParsePerformanceHtml(yearPerformance2TimesBefore),
            YearPerformance3TimesBefore = yearPerformance3TimesBefore == null ? new StockPerformance() : ParsePerformanceHtml(yearPerformance3TimesBefore),
            YearPerformance4TimesBefore = yearPerformance4TimesBefore == null ? new StockPerformance() : ParsePerformanceHtml(yearPerformance4TimesBefore),
            QuarterPerformance = quarterPerformance == null ? new StockPerformance() : ParsePerformanceHtml(quarterPerformance),
            QuarterPerformance1TimesBefore = quarterPerformance1TimesBefore == null ? new StockPerformance() : ParsePerformanceHtml(quarterPerformance1TimesBefore),
            QuarterPerformance2TimesBefore = quarterPerformance2TimesBefore == null ? new StockPerformance() : ParsePerformanceHtml(quarterPerformance2TimesBefore),
            QuarterPerformance3TimesBefore = quarterPerformance3TimesBefore == null ? new StockPerformance() : ParsePerformanceHtml(quarterPerformance3TimesBefore),
            QuarterPerformance4TimesBefore = quarterPerformance4TimesBefore == null ? new StockPerformance() : ParsePerformanceHtml(quarterPerformance4TimesBefore),
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

    private static string? ParseCodeHtml(IHtmlDocument doc)
    {
        string? code = doc
            .QuerySelectorAll("#stockinfo_i1 > div.si_i1_1 > h2 > span")
            .FirstOrDefault()
            ?.Text()
            ?.Trim();

        return code;
    }

    private static string? ParseMarketHtml(IHtmlDocument doc)
    {
        string? market = doc
            .QuerySelectorAll("#stockinfo_i1 span.market")
            .FirstOrDefault()
            ?.Text()
            ?.Trim();

        return market;
    }

    private static string? ParseNameHtml(IHtmlDocument doc, string code)
    {
        string? name = doc
            .QuerySelectorAll("#stockinfo_i1 > div.si_i1_1 > h2")
            .FirstOrDefault()
            ?.Text()
            ?.Replace(code, "")
            ?.Trim();

        return name;
    }

    private static string? ParseQuarterHtml(IHtmlDocument doc)
    {
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
        else if (quarter.EndsWith("1Q.jpg"))
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

        return quarter;
    }

    private static StockPerformance ParsePerformanceHtml(IElement row)
    {
        string NetSales = row
            .QuerySelectorAll("td:nth-child(2)")
            .First()
            .Text()
            .Replace(",", "")
            .Replace("－", "")
            .Trim();

        string OperatingProfit = row
            .QuerySelectorAll("td:nth-child(3)")
            .First()
            .Text()
            .Replace(",", "")
            .Replace("－", "")
            .Trim();

        string OrdinaryProfit = row
            .QuerySelectorAll("td:nth-child(4)")
            .First()
            .Text()
            .Replace(",", "")
            .Replace("－", "")
            .Trim();

        string Profit = row
            .QuerySelectorAll("td:nth-child(5)")
            .First()
            .Text()
            .Replace(",", "")
            .Replace("－", "")
            .Trim();

        string EarningsPerShare = row
            .QuerySelectorAll("td:nth-child(6)")
            .First()
            .Text()
            .Replace(",", "")
            .Replace("－", "")
            .Trim();

        StockPerformance performance = new StockPerformance()
        {
            NetSales = NetSales == "" ? null : decimal.Parse(NetSales),
            OperatingProfit = OperatingProfit == "" ? null : decimal.Parse(OperatingProfit),
            OrdinaryProfit = OrdinaryProfit == "" ? null : decimal.Parse(OrdinaryProfit),
            Profit = Profit == "" ? null : decimal.Parse(Profit),
            EarningsPerShare = EarningsPerShare == "" ? null : decimal.Parse(EarningsPerShare)
        };

        return performance;
    }
}

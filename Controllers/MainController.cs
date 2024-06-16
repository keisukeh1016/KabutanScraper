namespace KabutanScraper;

public class MainController
{
    public static async Task TdnetResults(string inputPath, string outputPath)
    {
        List<string> codes = FileService.ReadTdnetResultsCodes(inputPath);
        Console.WriteLine($"{codes.Count}件の銘柄情報を入力");

        List<string> htmlList = await HttpService.GetHtmlList(codes);
        htmlList = htmlList
            .Where(html => html != "")
            .ToList();

        List<Stock?> stocks = await HtmlParseService.ParseHtmlList(htmlList);
        stocks = stocks
            .Where(stock => stock != null)
            .ToList();

        FileService.OutputTdnetResultsCodes(stocks, outputPath);
        Console.WriteLine($"{stocks.Count}件の銘柄情報を出力");

        return;
    }
}

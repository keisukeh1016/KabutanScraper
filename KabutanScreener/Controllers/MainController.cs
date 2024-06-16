namespace KabutanScreener;

public static class MainController
{
    public static async Task Scrape(string inputPath, string outputPath)
    {
        //List<string> codes = FileService.ReadCodes(inputPath);
        //Console.WriteLine($"{codes.Count}件の銘柄情報を入力");

        //List<string> htmlList = await HttpService.GetHtmlList(codes);
        //htmlList = htmlList
        //    .Where(html => html != "")
        //    .ToList();

        //List<Stock?> stocks = await HtmlParseService.ParseHtmlList(htmlList);
        //stocks = stocks
        //    .Where(stock => stock != null)
        //    .ToList();

        //FileService.WriteResults(stocks, outputPath);
        //Console.WriteLine($"{stocks.Count}件の銘柄情報を出力");

        return;
    }
}

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KabutanScraper;

public class FileService
{
    public static void OutputTdnetResultsCodes(IList<Stock?> stocks, DateTime date)
    {
        string path = Path.Combine(Constants.IO.TdnetResultsCodesOutputPath, date.ToString("yyyy-MM-dd") + ".csv");

        var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(stocks, options);
        File.WriteAllText(path, jsonString);

        return;
    }

    public static List<string> ReadTdnetResultsCodes(DateTime date)
    {
        string path = Path.Combine(Constants.IO.TdnetResultsCodesInputPath, date.ToString("yyyy-MM-dd") + ".csv");

        if (!File.Exists(path))
        {
            Console.WriteLine("ファイルが存在しません。");
            return new List<string> { };
        }

        try
        {
            using (var sr = new StreamReader(path))
            {
                List<string> codes = sr
                    .ReadToEnd()
                    .Split(Environment.NewLine)
                    .Where(code => code != "")
                    .ToList();

                return codes;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new List<string> { };
        }
    }
}

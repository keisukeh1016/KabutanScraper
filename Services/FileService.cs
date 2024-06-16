using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KabutanScraper;

public static class FileService
{
    public static List<string> ReadCodes(string path)
    {
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

    public static void WriteResults(IList<Stock?> stocks, string path)
    {
        var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(stocks, options);
        File.WriteAllText(path, jsonString);

        return;
    }
}

using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using KabutanScraper;

namespace KabutanScreener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 引数0
            string inputPath = Path.GetFullPath(args[0]);

            // 引数1
            string outputPath = Path.GetFullPath(args[1]);

            // ファイル読み取り
            List<Stock>? stocks;
            using (FileStream fs = File.OpenRead(inputPath))
            {
                stocks = JsonSerializer.Deserialize<List<Stock>>(fs);
            }
            if (stocks == null)
            {
                return;
            }

            // スクリーニング
            Func<Stock, bool> operatingProfitIsMax = stock =>
            {
                if (stock.YearPerformance.OperatingProfitRate == null)
                {
                    return true;
                }

                List<decimal?> operatingProfits = new List<decimal?> {
                    stock.YearPerformance.OperatingProfitRate,
                    stock.YearPerformance1TimesBefore.OperatingProfitRate,
                    stock.YearPerformance2TimesBefore.OperatingProfitRate,
                    stock.YearPerformance3TimesBefore.OperatingProfitRate,
                    stock.YearPerformance4TimesBefore.OperatingProfitRate,
                };

                decimal? max = operatingProfits.Where(x => x != null).Max();
                bool isMax = max * 0.95m <= stock.YearPerformance.OperatingProfitRate;

                return isMax;
            };

            Func<Stock, bool> ordinaryProfitIsMax = stock =>
            {
                if (stock.YearPerformance.OrdinaryProfitRate == null)
                {
                    return true;
                }

                List<decimal?> ordinaryProfits = new List<decimal?> {
                    stock.YearPerformance.OrdinaryProfitRate,
                    stock.YearPerformance1TimesBefore.OrdinaryProfitRate,
                    stock.YearPerformance2TimesBefore.OrdinaryProfitRate,
                    stock.YearPerformance3TimesBefore.OrdinaryProfitRate,
                    stock.YearPerformance4TimesBefore.OrdinaryProfitRate,
                };

                decimal? max = ordinaryProfits.Where(x => x != null).Max();
                bool isMax = max * 0.95m == stock.YearPerformance.OrdinaryProfitRate;

                return isMax;
            };

            Func<Stock, bool> profitIsMax = stock =>
            {
                if (stock.YearPerformance.ProfitRate == null)
                {
                    return true;
                }

                List<decimal?> profits = new List<decimal?> {
                    stock.YearPerformance.ProfitRate,
                    stock.YearPerformance1TimesBefore.ProfitRate,
                    stock.YearPerformance2TimesBefore.ProfitRate,
                    stock.YearPerformance3TimesBefore.ProfitRate,
                    stock.YearPerformance4TimesBefore.ProfitRate,
                };

                decimal? max = profits.Where(x => x != null).Max();
                bool isMax = max * 0.95m == stock.YearPerformance.ProfitRate;

                return isMax;
            };

            List<Stock> screened = stocks
                .Where(x => operatingProfitIsMax(x) || ordinaryProfitIsMax(x) || profitIsMax(x))
                .ToList();

            // ファイル出力
            var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(screened, options);
            File.WriteAllText(outputPath, jsonString);

            return;
        }
    }
}

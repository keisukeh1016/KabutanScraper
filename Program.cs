namespace KabutanScraper;

class Program
{
    static void Main(string[] args)
    {
        // 引数0
        string inputPath = Path.GetFullPath(args[0]);

        // 引数1
        string outputPath = Path.GetFullPath(args[1]);

        // 実行
        Task task = MainController.TdnetResults(inputPath, outputPath);
        task.Wait();
    }
}

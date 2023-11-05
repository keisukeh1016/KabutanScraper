namespace KabutanScraper;

class Program
{
    static void Main(string[] args)
    {
        // ルーティング
        string mode = args[0].ToLower();
        DateTime date;
        bool isDate = DateTime.TryParse(args[1], out date);
        if (!isDate)
        {
            Console.WriteLine("日付の変換に失敗しました。");
            return;
        }

        switch (mode)
        {
            case "tdnet_results":
                Task task = MainController.TdnetResults(date);
                task.Wait();
                break;

            default:
                break;
        }
    }
}

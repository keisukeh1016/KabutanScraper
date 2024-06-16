namespace KabutanScraper;

public static class Constants
{
    public static class IO
    {
        public static readonly string TdnetResultsCodesInputPath = @"C:\Users\keisu\OneDrive\デスクトップ\stock\Out\TDnet_Results";
        public static readonly string TdnetResultsCodesOutputPath = @"C:\Users\keisu\OneDrive\デスクトップ\stock\Out\Kabutan_TDnet_Results";
    }

    public enum Market
    {
        東証Ｐ,
        東証Ｓ,
        東証Ｇ
    }

    public static List<string> MarketNames()
    {
        List<string> names = Enum
          .GetValues<Market>()
          .Select(m => m.ToString())
          .ToList();

        return names;
    }
}

namespace KabutanScraper;

public static class Constants
{
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

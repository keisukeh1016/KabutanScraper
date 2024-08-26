namespace KabutanScraper;

public static class HttpService
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> GetHtml(string code, int count = 1)
    {
        string url = $"https://kabutan.jp/stock/finance?code={code}";

        HttpResponseMessage? res = await GetHttpResponse(url);
        if (res == null)
        {
            return "";
        }

        string html = await res.Content.ReadAsStringAsync();
        return html;
    }

    public static async Task<List<string>> GetHtmlList(IList<string> codes)
    {
        List<Task<string>> tasks = new List<Task<string>> { };

        foreach (var code in codes)
        {
            Task<string> task = GetHtml(code);
            tasks.Add(task);

            await Task.Delay(500);
        }

        string[] htmlList = await Task.WhenAll(tasks);
        return htmlList.ToList();
    }

    public static async Task<HttpResponseMessage?> GetHttpResponse(string url, int count = 1)
    {
        HttpResponseMessage? res;

        try
        {
            res = await client.GetAsync(url);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        if (!res.IsSuccessStatusCode)
        {
            if (count < 5)
            {
                Console.WriteLine($"HTTPリクエストが失敗しました。count:{count}, url:{url}");
                await Task.Delay(count * 5 * 1000);
                res = await GetHttpResponse(url, count + 1);
            }
            else
            {
                Console.WriteLine($"HTTPリクエストが失敗しました。count:{count}, url:{url}");
                return null;
            }
        }

        return res;
    }
}

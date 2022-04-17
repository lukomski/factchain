using Newtonsoft.Json.Linq; 

class MainClass
{
    static readonly HttpClient client = new HttpClient();

    public static async Task FetchNewFact(string filename)
    {
        // make fetch
        HttpResponseMessage response = await client.GetAsync("https://catfact.ninja/fact/");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        // parse output
        dynamic jToken = JToken.Parse(responseBody);
        string message = jToken.fact;

        // append file
        using StreamWriter file = new(filename, append: true);
        await file.WriteLineAsync(message);
    }

    static void Main() {
        string fileName = "out.txt";
        FetchNewFact(fileName).GetAwaiter().GetResult();
    }
}
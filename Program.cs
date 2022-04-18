using Newtonsoft.Json.Linq; 


internal class WorkerClass {

    private readonly HttpClient _client;
    private readonly string _filename;
    public WorkerClass(HttpClient client, string filename) {
        _client = client;
        _filename = filename;
    }

    private async Task<string> GetFact()
    {
        HttpResponseMessage response = await _client.GetAsync("https://catfact.ninja/fact/");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task Save(string filename, string message)
    {
        using StreamWriter file = new(filename, append: true);
        await file.WriteLineAsync(message);
    }

    public void FetchNewFact()
    {
        var responseBody = GetFact().GetAwaiter().GetResult();

        // parse output
        dynamic jToken = JToken.Parse(responseBody);
        string message = jToken.fact;

        Save(_filename, message).GetAwaiter().GetResult();
    }
}

public class MainClass
{
    static readonly HttpClient client = new HttpClient();
    static void Main() {
        string fileName = "out.txt";
        WorkerClass workerClass = new WorkerClass(client, fileName);    
        workerClass.FetchNewFact();
    }
}
namespace Services;

public class FotbollService : IFotbollService
{
    private readonly HttpClient _httpClient;

    public FotbollService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetFixturesAsync(string teamId)
    {
        var response = await _httpClient.GetAsync("");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetResultsAsync(string teamId)
    {
        var response = await _httpClient.GetAsync("");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetPremierLeagueStandingsAsync()
    {
        var response = await _httpClient.GetAsync("");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSummaryAsync()
    {
        var fixtures = await GetFixturesAsync("");
        var results = await GetResultsAsync("");
        var standings = await GetPremierLeagueStandingsAsync();

        return $"Fixtures: {fixtures}\nResults: {results}\nStandings: {standings}";
    }
}
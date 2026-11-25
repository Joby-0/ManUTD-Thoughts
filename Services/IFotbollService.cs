namespace Services;

public interface IFotbollService
{
    public Task<string> GetFixturesAsync(string teamId);
    public Task<string> GetResultsAsync(string teamId);
    public Task<string> GetPremierLeagueStandingsAsync();
    public Task<string> GetSummaryAsync();
}
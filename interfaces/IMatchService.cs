
interface IMatchService
{
    Task<IEnumerable<Match>> GetAllMatches();
    Task<Match> GetMatchDetailsById(int id);
    Task<Match> CreateMatch(DateTime date, int homeTeamId, int awayTeamId, int homeTeamScore, int awayTeamScore, String location);
}
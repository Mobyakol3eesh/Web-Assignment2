
public interface IMatchService
{
    Task<IEnumerable<MatchReadDto>> GetAllMatches();
    Task<MatchReadDto> GetMatchDetailsById(int id);
    Task CreateMatch(CreateMatchDto dto);
    Task UpdateMatch(int id, UpdateMatchDto dto);
}
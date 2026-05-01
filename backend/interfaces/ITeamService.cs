

public interface ITeamService
{
    Task<IEnumerable<PlayerReadDto>> GetALLTeamPlayers(int teamId);

    Task<TeamReadDto> GetTeamDetailsById(int id);

    Task CreateTeam(CreateTeamDto dto);
    Task UpdateTeam(int id, UpdateTeamDto dto);
    Task<PlayerReadDto> GetMostValuablePlayerinTeam(int teamId);

    Task<IEnumerable<TeamReadDto>> GetAllTeams();

    Task<IEnumerable<MatchReadDto>> GetTeamMatches(int teamId);

    Task<CoachReadDto> GetTeamCoach(int teamId);


    
}
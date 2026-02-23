

public interface ITeamService
{
    IEnumerable<Player> GetALLTeamPlayers(int teamId);

    Team GetTeamDetailsById(int id);
    Player GetMostValuablePlayerinTeam(int teamId);

    IEnumerable<Team> GetAllTeams();


    
}

using Microsoft.EntityFrameworkCore;

public class TeamService : ITeamService
{
   
    
    
    FootballContext footballContext;
    public TeamService(FootballContext footballContext)
    {
        
        this.footballContext = footballContext;

        
    }

    public async Task<IEnumerable<Team>> GetAllTeams()
    {
        return await footballContext.teams.Include(t => t.Players).ToListAsync();
    }


    public async Task<IEnumerable<Player>> GetALLTeamPlayers(int teamId)
    {
        return await footballContext.players.Where(p => p.TeamId == teamId).ToListAsync();
       
    }
    
    
    public async Task<Team> GetTeamDetailsById(int id)
    {
        var team = await footballContext.teams.Where(t => t.Id == id).Include(t => t.Players).FirstOrDefaultAsync();
        if (team == null)
        {
            throw new Exception($"Team with ID {id} not found.");
        }
        return team;
    }

    public async Task<Player> GetMostValuablePlayerinTeam(int teamID)
    {
        var team = await footballContext.teams.Where(t => t.Id == teamID).Include(t => t.Players).FirstOrDefaultAsync();
        if (team == null)
        {
            throw new Exception($"Team with ID {teamID} not found.");
        }
        var mostValuablePlayer = team.Players.OrderByDescending(p => p.MarketValue).FirstOrDefault();
        if (mostValuablePlayer == null)
        {
            throw new Exception($"No players found in Team with ID {teamID}.");
        }
        return mostValuablePlayer;
    }
}
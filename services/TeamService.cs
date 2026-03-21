
using Microsoft.EntityFrameworkCore;

public class TeamService : ITeamService
{
   
    
    
    private readonly TunaLeagueContext tunaLeagueContext;
    public TeamService(TunaLeagueContext tunaLeagueContext)
    {
        
        this.tunaLeagueContext = tunaLeagueContext;

        
    }

    public async Task<IEnumerable<Team>> GetAllTeams()
    {
        return await tunaLeagueContext.Teams.AsNoTracking().ToListAsync();
    }


    public async Task<IEnumerable<Player>> GetALLTeamPlayers(int teamId)
    {
        return await tunaLeagueContext.Players.AsNoTracking().Where(p => p.TeamId == teamId).ToListAsync();
       
    }
    
    
    public async Task<Team> GetTeamDetailsById(int id)
    {
        var team = await tunaLeagueContext.Teams
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
        if (team == null)
        {
            throw new Exception($"Team with ID {id} not found.");
        }
        return team;
    }

    public async Task<Player> GetMostValuablePlayerinTeam(int teamID)
    {
        var teamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == teamID);

        if (!teamExists)
        {
            throw new Exception($"Team with ID {teamID} not found.");
        }

        var mostValuablePlayer = await tunaLeagueContext.Players
            .AsNoTracking()
            .Where(p => p.TeamId == teamID)
            .OrderByDescending(p => p.MarketValue)
            .FirstOrDefaultAsync();

        if (mostValuablePlayer == null)
        {
            throw new Exception($"No players found in Team with ID {teamID}.");
        }
        return mostValuablePlayer;
    }
}
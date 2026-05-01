
using Microsoft.EntityFrameworkCore;

public class TeamService : ITeamService
{
   
    
    
    private readonly TunaLeagueContext tunaLeagueContext;
    public TeamService(TunaLeagueContext tunaLeagueContext)
    {
        
        this.tunaLeagueContext = tunaLeagueContext;

        
    }

    public async Task CreateTeam(CreateTeamDto dto)
    {
        var nextTeamId = (await tunaLeagueContext.Teams
            .Select(t => (int?)t.Id)
            .MaxAsync() ?? 0) + 1;

        var newTeam = new Team
        {
            Id = nextTeamId,
            Name = dto.Name,
            Points = 0,
        };
        tunaLeagueContext.Teams.Add(newTeam);
        await tunaLeagueContext.SaveChangesAsync();
    }
    public async Task<CoachReadDto> GetTeamCoach(int teamId)
    {
        var teamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == teamId);

        if (!teamExists)
        {
            throw new Exception($"Team with ID {teamId} not found.");
        }

        var coach = await tunaLeagueContext.Coaches
            .AsNoTracking()
            .Where(c => c.TeamId == teamId)
            .Select(c => new CoachReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Age = c.Age,
                ExperienceYrs = c.ExperienceYrs,
                TeamName = c.Team != null ? c.Team.Name : string.Empty
            }).FirstOrDefaultAsync();

        if (coach == null)
        {
            throw new Exception($"No coach assigned to Team with ID {teamId}.");
        }
        return coach;
    }
    public async Task UpdateTeam(int id, UpdateTeamDto dto)
    {
        var team = await tunaLeagueContext.Teams
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
        {
            throw new Exception("Team not found");
        }

        team.Name = dto.Name;
        team.Points = dto.Points;

        await tunaLeagueContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<MatchReadDto>> GetTeamMatches(int teamId)
    {
        var goalCounts = await tunaLeagueContext.Goals
            .AsNoTracking()
            .GroupBy(g => new { g.MatchId, g.TeamId })
            .Select(g => new { g.Key.MatchId, g.Key.TeamId, Count = g.Count() })
            .ToListAsync();

        var goalCountLookup = goalCounts.ToDictionary(
            x => (x.MatchId, x.TeamId),
            x => x.Count);

        var matches = await tunaLeagueContext.Matches
            .AsNoTracking()
            .Where(match => match.HomeTeamId == teamId || match.AwayTeamId == teamId)
            .Select(match => new
            {
                match.Id,
                match.Date,
                match.Location,
                HomeTeamName = match.HomeTeam != null ? match.HomeTeam.Name : string.Empty,
                match.HomeTeamScore,
                match.HomeTeamId,
                AwayTeamName = match.AwayTeam != null ? match.AwayTeam.Name : string.Empty,
                match.AwayTeamScore,
                match.AwayTeamId
            })
            .ToListAsync();

        return matches.Select(match => new MatchReadDto
        {
            Id = match.Id,
            Date = match.Date,
            Location = match.Location,
            HomeTeamName = match.HomeTeamName,
            HomeTeamScore = match.HomeTeamScore,
            HomeTeamGoals = goalCountLookup.TryGetValue((match.Id, match.HomeTeamId), out var homeGoals)
                ? homeGoals
                : 0,
            AwayTeamName = match.AwayTeamName,
            AwayTeamScore = match.AwayTeamScore,
            AwayTeamGoals = goalCountLookup.TryGetValue((match.Id, match.AwayTeamId), out var awayGoals)
                ? awayGoals
                : 0
        }).ToList();
    }

    public async Task<IEnumerable<TeamReadDto>> GetAllTeams()
    {
        var goalCounts = await tunaLeagueContext.Goals
            .AsNoTracking()
            .GroupBy(g => g.TeamId)
            .Select(g => new { TeamId = g.Key, Count = g.Count() })
            .ToListAsync();

        var goalCountDict = goalCounts.ToDictionary(x => x.TeamId, x => x.Count);

        var tdtos = await tunaLeagueContext.Teams.AsNoTracking().Select(t => new TeamReadDto
        {
            Id = t.Id,
            Name = t.Name,
            Points = t.Points,
            Goals = goalCountDict.ContainsKey(t.Id) ? goalCountDict[t.Id] : 0,
            Players = t.Players.Select(p => new PlayerReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Position = p.Position,
                MarketValue = p.MarketValue,
                TeamName = t.Name
            }).ToList(),
            CoachName = t.Coach != null ? t.Coach.Name : string.Empty,
        }
                    ).ToListAsync();
        return tdtos;
    }



    public async Task<IEnumerable<PlayerReadDto>> GetALLTeamPlayers(int teamId)
    {
       var tplayersdtos = await tunaLeagueContext.Players.AsNoTracking().Where(p => p.TeamId == teamId).Select(p => new PlayerReadDto
       {
           Id = p.Id,
           Name = p.Name,
           Age = p.Age,
           Position = p.Position,
           MarketValue = p.MarketValue,
          TeamName = p.Team != null ? p.Team.Name : string.Empty
       }).ToListAsync();
       return tplayersdtos;
    }
    
    
    public async Task<TeamReadDto> GetTeamDetailsById(int id)
    {
        var goalCount = await tunaLeagueContext.Goals
            .AsNoTracking()
            .CountAsync(g => g.TeamId == id);

        var team = await tunaLeagueContext.Teams.AsNoTracking()
        .Where(t => t.Id == id)
        .Select(t => new TeamReadDto
        {
            Id = t.Id,
            Name = t.Name,
            Points = t.Points,
            Goals = goalCount,
            Players = t.Players.Select(p => new PlayerReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Position = p.Position,
                MarketValue = p.MarketValue,
                TeamName = t.Name
            }).ToList(),
            CoachName = t.Coach != null ? t.Coach.Name : string.Empty,
        }).FirstOrDefaultAsync();
        return team ?? throw new Exception("Team not found");
    }

    public async Task<PlayerReadDto> GetMostValuablePlayerinTeam(int teamID)
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
            
            .Select(p => new PlayerReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Position = p.Position,
                MarketValue = p.MarketValue,
                TeamName = p.Team != null ? p.Team.Name : string.Empty
            }).FirstOrDefaultAsync();
                

        if (mostValuablePlayer == null)
        {
            throw new Exception($"No players found in Team with ID {teamID}.");
        }
        return mostValuablePlayer;
    }

    
}
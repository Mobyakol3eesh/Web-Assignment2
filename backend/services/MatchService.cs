


using Microsoft.EntityFrameworkCore;

public class MatchService : IMatchService
{
    private readonly TunaLeagueContext tunaLeagueContext;

    public MatchService(TunaLeagueContext tunaLeagueContext)
    {
        this.tunaLeagueContext = tunaLeagueContext;
    }

    public async Task<IEnumerable<MatchReadDto>> GetAllMatches()
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
    public async Task CreateMatch(CreateMatchDto dto)
    {
        if (dto.HomeTeamId == dto.AwayTeamId)
        {
            throw new Exception("Home team and away team must be different.");
        }

        var homeTeamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.HomeTeamId);

        var awayTeamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.AwayTeamId);

        if (!homeTeamExists || !awayTeamExists)
        {
            throw new Exception("Invalid team id provided for match creation.");
        }

        var match = new Match
        {
            Id = (await tunaLeagueContext.Matches
                .Select(m => (int?)m.Id)
                .MaxAsync() ?? 0) + 1,
            Date = dto.Date,
            Location = dto.Location,
            HomeTeamId = dto.HomeTeamId,
            AwayTeamId = dto.AwayTeamId,
            HomeTeamScore = dto.HomeTeamScore,
            AwayTeamScore = dto.AwayTeamScore
        };
        tunaLeagueContext.Matches.Add(match);
        await tunaLeagueContext.SaveChangesAsync();
    }


    public async Task<MatchReadDto> GetMatchDetailsById(int id)
    {
        var match = await tunaLeagueContext.Matches
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new
            {
                m.Id,
                m.Date,
                m.Location,
                HomeTeamName = m.HomeTeam != null ? m.HomeTeam.Name : string.Empty,
                m.HomeTeamScore,
                m.HomeTeamId,
                AwayTeamName = m.AwayTeam != null ? m.AwayTeam.Name : string.Empty,
                m.AwayTeamScore,
                m.AwayTeamId
            })
            .FirstOrDefaultAsync();

        if (match == null)
            throw new Exception("Match not found");

        var homeGoalCount = await tunaLeagueContext.Goals
            .AsNoTracking()
            .CountAsync(g => g.MatchId == id && g.TeamId == match.HomeTeamId);

        var awayGoalCount = await tunaLeagueContext.Goals
            .AsNoTracking()
            .CountAsync(g => g.MatchId == id && g.TeamId == match.AwayTeamId);

        return new MatchReadDto
        {
            Id = match.Id,
            Date = match.Date,
            Location = match.Location,
            HomeTeamName = match.HomeTeamName,
            HomeTeamScore = match.HomeTeamScore,
            HomeTeamGoals = homeGoalCount,
            AwayTeamName = match.AwayTeamName,
            AwayTeamScore = match.AwayTeamScore,
            AwayTeamGoals = awayGoalCount
        };
    }

    public async Task UpdateMatch(int id, UpdateMatchDto dto)
    {
        if (dto.HomeTeamId == dto.AwayTeamId)
        {
            throw new Exception("Home team and away team must be different.");
        }

        var match = await tunaLeagueContext.Matches
            .FirstOrDefaultAsync(m => m.Id == id);

        if (match == null)
        {
            throw new Exception("Match not found");
        }

        var homeTeamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.HomeTeamId);

        var awayTeamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.AwayTeamId);

        if (!homeTeamExists || !awayTeamExists)
        {
            throw new Exception("Invalid team id provided for match update.");
        }

        match.Date = dto.Date;
        match.Location = dto.Location;
        match.HomeTeamId = dto.HomeTeamId;
        match.AwayTeamId = dto.AwayTeamId;
        match.HomeTeamScore = dto.HomeTeamScore;
        match.AwayTeamScore = dto.AwayTeamScore;

        await tunaLeagueContext.SaveChangesAsync();
    }

    public async Task DeleteMatch(int id)
    {
        var match = await tunaLeagueContext.Matches.FirstOrDefaultAsync(m => m.Id == id);
        if (match == null)
            throw new Exception("Match not found");

        // Remove goals for the match
        var goals = tunaLeagueContext.Goals.Where(g => g.MatchId == id);
        tunaLeagueContext.Goals.RemoveRange(goals);

        // Remove player stats for the match
        var stats = tunaLeagueContext.PlayerStats.Where(ps => ps.MatchId == id);
        tunaLeagueContext.PlayerStats.RemoveRange(stats);

        // Remove the match
        tunaLeagueContext.Matches.Remove(match);
        await tunaLeagueContext.SaveChangesAsync();
    }

    
}
    
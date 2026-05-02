using Microsoft.EntityFrameworkCore;

public class GoalService : IGoalService
{
    private readonly TunaLeagueContext tunaLeagueContext;

    public GoalService(TunaLeagueContext tunaLeagueContext)
    {
        this.tunaLeagueContext = tunaLeagueContext;
    }

    public async Task<IEnumerable<GoalReadDto>> GetAllGoals()
    {
        return await tunaLeagueContext.Goals
            .AsNoTracking()
            .Select(goal => new GoalReadDto
            {
                Id = goal.Id,
                TimeScored = goal.TimeScored,
                ScorerName = goal.ScorerName,
                PlayerId = goal.PlayerId,
                MatchId = goal.MatchId,
                MatchLocation = goal.Match != null ? goal.Match.Location : string.Empty,
                TeamId = goal.TeamId,
                TeamName = goal.Team != null ? goal.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<GoalReadDto> GetGoalById(int id)
    {
        var goal = await tunaLeagueContext.Goals
            .AsNoTracking()
            .Where(g => g.Id == id)
            .Select(goal => new GoalReadDto
            {
                Id = goal.Id,
                TimeScored = goal.TimeScored,
                ScorerName = goal.ScorerName,
                PlayerId = goal.PlayerId,
                MatchId = goal.MatchId,
                MatchLocation = goal.Match != null ? goal.Match.Location : string.Empty,
                TeamId = goal.TeamId,
                TeamName = goal.Team != null ? goal.Team.Name : string.Empty
            })
            .FirstOrDefaultAsync();

        return goal ?? throw new Exception("Goal not found");
    }

    public async Task<IEnumerable<GoalReadDto>> GetGoalsByMatch(int matchId)
    {
        return await tunaLeagueContext.Goals
            .AsNoTracking()
            .Where(goal => goal.MatchId == matchId)
            .Select(goal => new GoalReadDto
            {
                Id = goal.Id,
                TimeScored = goal.TimeScored,
                ScorerName = goal.ScorerName,
                PlayerId = goal.PlayerId,
                MatchId = goal.MatchId,
                MatchLocation = goal.Match != null ? goal.Match.Location : string.Empty,
                TeamId = goal.TeamId,
                TeamName = goal.Team != null ? goal.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<GoalReadDto>> GetGoalsByTeam(int teamId)
    {
        return await tunaLeagueContext.Goals
            .AsNoTracking()
            .Where(goal => goal.TeamId == teamId)
            .Select(goal => new GoalReadDto
            {
                Id = goal.Id,
                TimeScored = goal.TimeScored,
                ScorerName = goal.ScorerName,
                PlayerId = goal.PlayerId,
                MatchId = goal.MatchId,
                MatchLocation = goal.Match != null ? goal.Match.Location : string.Empty,
                TeamId = goal.TeamId,
                TeamName = goal.Team != null ? goal.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task CreateGoal(CreateGoalDto dto)
    {
        var player = await tunaLeagueContext.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(player => player.Id == dto.PlayerId);

        if (player == null)
        {
            throw new Exception("Player not found.");
        }

        var match = await tunaLeagueContext.Matches
            .AsNoTracking()
            .FirstOrDefaultAsync(match => match.Id == dto.MatchId);

        if (match == null)
        {
            throw new Exception("Match not found.");
        }

        if (player.TeamId != dto.TeamId)
        {
            throw new Exception("Player must belong to the goal team.");
        }

        if (match.HomeTeamId != dto.TeamId && match.AwayTeamId != dto.TeamId)
        {
            throw new Exception("Goal team must be one of the match teams.");
        }

        var goal = new Goal
        {
            TimeScored = dto.TimeScored == default ? DateTime.UtcNow : dto.TimeScored,
            ScorerName = player.Name,
            PlayerId = dto.PlayerId,
            MatchId = dto.MatchId,
            TeamId = dto.TeamId
        };

        tunaLeagueContext.Goals.Add(goal);
        await tunaLeagueContext.SaveChangesAsync();
    }

    public async Task DeleteGoal(int id)
    {
        var goal = await tunaLeagueContext.Goals.FirstOrDefaultAsync(g => g.Id == id);
        if (goal == null)
        {
            throw new Exception("Goal not found");
        }

        tunaLeagueContext.Goals.Remove(goal);
        await tunaLeagueContext.SaveChangesAsync();
    }
}
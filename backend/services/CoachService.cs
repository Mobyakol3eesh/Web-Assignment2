using Microsoft.EntityFrameworkCore;

public class CoachService : ICoachService
{
    private readonly TunaLeagueContext tunaLeagueContext;

    public CoachService(TunaLeagueContext tunaLeagueContext)
    {
        this.tunaLeagueContext = tunaLeagueContext;
    }

    public async Task<IEnumerable<CoachReadDto>> GetAllCoaches()
    {
        return await tunaLeagueContext.Coaches
            .AsNoTracking()
            .Select(c => new CoachReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Age = c.Age,
                ExperienceYrs = c.ExperienceYrs,
                TeamName = c.Team != null ? c.Team.Name : string.Empty
            })
            .ToListAsync();
    }

    public async Task<CoachReadDto> GetCoachDetailsById(int id)
    {
        var coach = await tunaLeagueContext.Coaches
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CoachReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Age = c.Age,
                ExperienceYrs = c.ExperienceYrs,
                TeamName = c.Team != null ? c.Team.Name : string.Empty
            })
            .FirstOrDefaultAsync();

        return coach ?? throw new Exception("Coach not found");
    }

    public async Task AddCoach(CreateCoachDto dto)
    {
        if (dto.TeamId.HasValue)
        {
            var teamExists = await tunaLeagueContext.Teams
                .AsNoTracking()
                .AnyAsync(t => t.Id == dto.TeamId.Value);

            if (!teamExists)
            {
                throw new Exception("Team not found");
            }

            var teamHasCoach = await tunaLeagueContext.Coaches
                .AsNoTracking()
                .AnyAsync(c => c.TeamId == dto.TeamId.Value);

            if (teamHasCoach)
            {
                throw new Exception("This team already has a coach.");
            }
        }

        var nextCoachId = (await tunaLeagueContext.Coaches
            .Select(c => (int?)c.Id)
            .MaxAsync() ?? 0) + 1;

        var coach = new Coach
        {
            Id = nextCoachId,
            Name = dto.Name,
            Age = dto.Age,
            ExperienceYrs = dto.ExperienceYrs,
            TeamId = dto.TeamId
        };

        tunaLeagueContext.Coaches.Add(coach);
        await tunaLeagueContext.SaveChangesAsync();
    }

    public async Task UpdateCoach(int id, UpdateCoachDto dto)
    {
        var coach = await tunaLeagueContext.Coaches
            .FirstOrDefaultAsync(c => c.Id == id);

        if (coach == null)
        {
            throw new Exception("Coach not found");
        }

        if (dto.TeamId.HasValue)
        {
            var teamExists = await tunaLeagueContext.Teams
                .AsNoTracking()
                .AnyAsync(t => t.Id == dto.TeamId.Value);

            if (!teamExists)
            {
                throw new Exception("Team not found");
            }

            var teamHasAnotherCoach = await tunaLeagueContext.Coaches
                .AsNoTracking()
                .AnyAsync(c => c.TeamId == dto.TeamId.Value && c.Id != id);

            if (teamHasAnotherCoach)
            {
                throw new Exception("This team already has a coach.");
            }
        }
        
        coach.Name = dto.Name ?? coach.Name ;
        coach.Age = dto.Age != 0 ? dto.Age : coach.Age;
        coach.ExperienceYrs = dto.ExperienceYrs != 0 ? dto.ExperienceYrs : coach.ExperienceYrs;
        coach.TeamId = dto.TeamId ?? coach.TeamId;

        await tunaLeagueContext.SaveChangesAsync();
    }
}
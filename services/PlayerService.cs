

using Microsoft.EntityFrameworkCore;


public class PlayerService : IPlayerService
{
    private readonly TunaLeagueContext tunaLeagueContext;
    public PlayerService(TunaLeagueContext tunaLeagueContext)
    {
        this.tunaLeagueContext = tunaLeagueContext;
    }
    public async Task<PlayerReadDto> GetPlayerDetailsById(int id)
    {
        var player = await tunaLeagueContext.Players.AsNoTracking()
       
        .Select(p => new PlayerReadDto
        {
            Id = p.Id,
            Name = p.Name,
            MarketValue = p.MarketValue,
            TeamName = p.Team != null ? p.Team.Name : string.Empty
        }).FirstOrDefaultAsync(p => p.Id == id);
        return player ?? throw new Exception("Player not found");
    }
    public async Task AddPlayer(CreatePlayerDto dto)
    {
        var nextPlayerId = (await tunaLeagueContext.Players
            .Select(p => (int?)p.Id)
            .MaxAsync() ?? 0) + 1;

        var newPlayer = new Player
        {
            Id = nextPlayerId,
            Name = dto.Name,
            MarketValue = dto.MarketValue,
            Age = dto.Age,
            Position = dto.Position,
            TeamId = dto.TeamId,
        };
        tunaLeagueContext.Players.Add(newPlayer);
        await tunaLeagueContext.SaveChangesAsync();
    }

    public async Task UpdatePlayer(int id, UpdatePlayerDto dto)
    {
        var player = await tunaLeagueContext.Players
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            throw new Exception("Player not found");
        }

        var teamExists = await tunaLeagueContext.Teams
            .AsNoTracking()
            .AnyAsync(t => t.Id == dto.TeamId);

        if (!teamExists)
        {
            throw new Exception("Team not found");
        }

        player.Name = dto.Name ?? player.Name;
        player.MarketValue = dto.MarketValue != 0 ? dto.MarketValue : player.MarketValue;
        player.Age = dto.Age != 0 ? dto.Age : player.Age;
        player.Position = dto.Position ?? player.Position;
        player.TeamId = dto.TeamId != 0 ? dto.TeamId : player.TeamId;

        await tunaLeagueContext.SaveChangesAsync();
    }
   

    public async Task<IEnumerable<PlayerReadDto>> GetAllPlayers()
    {
        var Players = await tunaLeagueContext.Players
        .AsNoTracking()
        .Select(p => new PlayerReadDto
                     
                 
         {
             Id = p.Id,
             Name = p.Name,
             Position = p.Position,
              Age = p.Age,
             MarketValue = p.MarketValue,
             TeamName = p.Team != null ? p.Team.Name : string.Empty
         }
         ).ToListAsync();
        return Players;
    }

    public async Task<IEnumerable<PlayerStatsReadDto>> GetAllPlayerStats()
    {
        return await tunaLeagueContext.PlayerStats
            .AsNoTracking()
            .Select(ps => new PlayerStatsReadDto
            {
                Id = ps.Id,
                Goals = ps.Goals,
                Assists = ps.Assists,
                ShotsOnTarget = ps.ShotsOnTarget,
                Touches = ps.Touches,
                PassesCompleted = ps.PassesCompleted,
                Score = ps.Score,
                PlayerId = ps.PlayerId,
                MatchId = ps.MatchId,
                PlayerName = ps.Player != null ? ps.Player.Name : string.Empty,
                MatchDate = ps.Match != null ? ps.Match.Date : default,
                MatchLocation = ps.Match != null ? ps.Match.Location : string.Empty
            })
            .ToListAsync();
    }

    public async Task<PlayerStatsReadDto> GetPlayerStatsById(int id)
    {
        var playerStats = await tunaLeagueContext.PlayerStats
            .AsNoTracking()
            .Where(ps => ps.Id == id)
            .Select(ps => new PlayerStatsReadDto
            {
                Id = ps.Id,
                Goals = ps.Goals,
                Assists = ps.Assists,
                ShotsOnTarget = ps.ShotsOnTarget,
                Touches = ps.Touches,
                PassesCompleted = ps.PassesCompleted,
                Score = ps.Score,
                PlayerId = ps.PlayerId,
                MatchId = ps.MatchId,
                PlayerName = ps.Player != null ? ps.Player.Name : string.Empty,
                MatchDate = ps.Match != null ? ps.Match.Date : default,
                MatchLocation = ps.Match != null ? ps.Match.Location : string.Empty
            })
            .FirstOrDefaultAsync();

        return playerStats ?? throw new Exception("Player stats not found");
    }

    public async Task AddPlayerStats(CreatePlayerStatsDto dto)
    {
        var playerExists = await tunaLeagueContext.Players
            .AsNoTracking()
            .AnyAsync(p => p.Id == dto.PlayerId);

        var matchExists = await tunaLeagueContext.Matches
            .AsNoTracking()
            .AnyAsync(m => m.Id == dto.MatchId);

        if (!playerExists || !matchExists)
        {
            throw new Exception("Invalid player or match id.");
        }

        var playerStats = new PlayerStats
        {
            Goals = dto.Goals,
            Assists = dto.Assists,
            ShotsOnTarget = dto.ShotsOnTarget,
            Touches = dto.Touches,
            PassesCompleted = dto.PassesCompleted,
            Score = dto.Score,
            PlayerId = dto.PlayerId,
            MatchId = dto.MatchId,
            Player = null!,
            Match = null!
        };

        tunaLeagueContext.PlayerStats.Add(playerStats);
        await tunaLeagueContext.SaveChangesAsync();
    }

    public async Task UpdatePlayerStats(int id, UpdatePlayerStatsDto dto)
    {
        var playerStats = await tunaLeagueContext.PlayerStats
            .FirstOrDefaultAsync(ps => ps.Id == id);

        if (playerStats == null)
        {
            throw new Exception("Player stats not found");
        }

        playerStats.Goals = dto.Goals;
        playerStats.Assists = dto.Assists;
        playerStats.ShotsOnTarget = dto.ShotsOnTarget;
        playerStats.Touches = dto.Touches;
        playerStats.PassesCompleted = dto.PassesCompleted;
        playerStats.Score = dto.Score;

        await tunaLeagueContext.SaveChangesAsync();
    }
}


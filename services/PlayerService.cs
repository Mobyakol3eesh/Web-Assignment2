

using Microsoft.EntityFrameworkCore;


public class PlayerService : IPlayerService
{
    
    

   TunaLeagueContext tunaLeagueContext;
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
            TeamReadDto = 
                 new TeamReadDto
                {
                    Id = p.Team.Id,
                    Name = p.Team.Name,
                    Points = p.Team.Points
                }
        }).FirstOrDefaultAsync(p => p.Id == id);
        return player ?? throw new Exception("Player not found");
    }
    public async Task AddPlayerAsync(CreatePlayerDto dto)
    {
        var newPlayer = new Player
        {
            Id = tunaLeagueContext.Players.Max(p => p.Id) + 1,
            Name = dto.Name,
            MarketValue = dto.MarketValue,
            TeamId = dto.TeamId,
        };
        tunaLeagueContext.Players.Add(newPlayer);
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
             MarketValue = p.MarketValue,
             TeamReadDto =  new TeamReadDto
                 {
                     Id = p.Team.Id,
                     Name = p.Team.Name,
                     Points = p.Team.Points
                 }
         }
         ).ToListAsync();
        return Players;
    }
}


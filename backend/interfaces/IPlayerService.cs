


public interface IPlayerService
{
   
    Task<IEnumerable<PlayerReadDto>> GetAllPlayers();
    Task<PlayerReadDto> GetPlayerDetailsById(int id);

    Task AddPlayer(CreatePlayerDto dto);
    Task UpdatePlayer(int id, UpdatePlayerDto dto);

    Task<IEnumerable<PlayerStatsReadDto>> GetAllPlayerStats();
    Task<PlayerStatsReadDto> GetPlayerStatsById(int id);
    Task AddPlayerStats(CreatePlayerStatsDto dto);
    Task UpdatePlayerStats(int id, UpdatePlayerStatsDto dto);

}
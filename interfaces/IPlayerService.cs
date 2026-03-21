


public interface IPlayerService
{
   
    Task<IEnumerable<PlayerReadDto>> GetAllPlayers();
    Task<PlayerReadDto> GetPlayerDetailsById(int id);

    Task AddPlayerAsync(CreatePlayerDto dto);

}
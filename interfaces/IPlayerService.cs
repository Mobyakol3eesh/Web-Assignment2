


public interface IPlayerService
{
    public event EventHandler<PlayerAddedEventArgs>? PlayerAdded;
    IEnumerable<Player> GetAllPlayers();
    Player GetPlayerDetailsById(int id);

    void AddPlayer(String name, int marketValue, int? teamID);

}
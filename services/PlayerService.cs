
public class PlayerService : IPlayerService
{
    public event EventHandler<PlayerAddedEventArgs>? PlayerAdded;
    private readonly List<Player> players = new List<Player>
    {
        new Player { Id = 1, Name = "Player 1", TeamId = 1 , MarketValue = 1000000},
        new Player { Id = 2, Name = "Player 2", TeamId = 1  , MarketValue = 1500000},
        new Player { Id = 3, Name = "Player 3", TeamId = 2 , MarketValue = 800000},
        new Player { Id = 4, Name = "Player 4", TeamId = 2 , MarketValue = 1200000},
        new Player { Id = 5, Name = "Player 5", TeamId = 3 , MarketValue = 900000},
        new Player { Id = 6, Name = "Player 6", TeamId = 3 , MarketValue = 1100000}
    };

   

    public Player GetPlayerDetailsById(int id)
    {
        var player = players.FirstOrDefault(p => p.Id == id);
        if (player == null)
        {
            throw new Exception($"Player with ID {id} not found.");
        }
        return player;
    }
    public void AddPlayer(String name, int marketValue, int? teamID)
    {
        var newPlayer = new Player
        {
            Id = players.Max(p => p.Id) + 1,
            Name = name,
            MarketValue = marketValue,
            TeamId = teamID
        };
        players.Add(newPlayer);
        OnPlayerAdded(new PlayerAddedEventArgs(newPlayer));
    }
    protected virtual void OnPlayerAdded(PlayerAddedEventArgs e)
    {
        PlayerAdded?.Invoke(this, e);  
    }

    public IEnumerable<Player> GetAllPlayers()
    {
        return players;
    }
}

public class PlayerAddedEventArgs : EventArgs
{
    public Player AddedPlayer { get; set; }

    public PlayerAddedEventArgs(Player player)
    {
        AddedPlayer = player;
    }
}
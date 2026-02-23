
public class TeamService : ITeamService
{
   
    private readonly IPlayerService playerService;
    private List<Team> teams = new List<Team>
    {
        new Team { Id = 1, Name = "Team A", Points = 10, Players = new List<Player>() },
        new Team { Id = 2, Name = "Team B", Points = 20, Players = new List<Player>() },
        new Team { Id = 3, Name = "Team C", Points = 15, Players = new List<Player>() }
    };

    public TeamService(IPlayerService playerService)
    {
        this.playerService = playerService;
        
        foreach (var team in teams)
        {
            team.Players = this.playerService.GetAllPlayers().Where(p => p.TeamId == team.Id).ToList();
        }
        playerService.PlayerAdded += OnPlayerAdded;
    }

    public IEnumerable<Team> GetAllTeams()
    {
        return teams;
    }


    public IEnumerable<Player> GetALLTeamPlayers(int teamId)
    {
        var team = teams.FirstOrDefault(t => t.Id == teamId);
        if (team == null)
        {
            throw new Exception($"Team with ID {teamId} not found.");
        }
        return team.Players;
       
    }
    
    private void OnPlayerAdded(object? sender, PlayerAddedEventArgs e)
    {
        var player = e.AddedPlayer;

       
        var team = teams.FirstOrDefault(t => t.Id == player.TeamId);
        if (team != null)
        {
            team.Players.Add(player);
        }
    }
    public Team GetTeamDetailsById(int id)
    {
        var team = teams.FirstOrDefault(t => t.Id == id);
        if (team == null)
        {
            throw new Exception($"Team with ID {id} not found.");
        }
        return team;
    }

    public Player GetMostValuablePlayerinTeam(int teamID)
    {
        var team = teams.FirstOrDefault(t => t.Id == teamID);
        if (team == null)
        {
            throw new Exception($"Team with ID {teamID} not found.");
        }
        var mostValuablePlayer = team.Players.OrderByDescending(p => p.MarketValue).FirstOrDefault();
        if (mostValuablePlayer == null)
        {
            throw new Exception($"No players found in Team with ID {teamID}.");
        }
        return mostValuablePlayer;
    }
}


using Microsoft.AspNetCore.Mvc;

public class TeamController : Controller
{
    private ITeamService teamService;

    public TeamController(ITeamService teamService)
    {
        this.teamService = teamService;
    }

    [HttpGet("teams/MVP/{teamId}")]
    public ActionResult<Player> GetMostValuablePlayerInTeam(int teamId)
    {
        try
        {
            var mostValuablePlayer = teamService.GetMostValuablePlayerinTeam(teamId);
            return Ok(mostValuablePlayer);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams/{id}")]
    public ActionResult<Team> GetTeamDetailsById(int id)
    {
        try
        {
            var team = teamService.GetTeamDetailsById(id);
            return Ok(team);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams/teamplayers/{teamId}")]
    public ActionResult<IEnumerable<Player>> GetAllTeamPlayers(int teamId)
    {
        try
        {
            var players = teamService.GetALLTeamPlayers(teamId);
            return Ok(players);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams")]
    public ActionResult<IEnumerable<Team>> GetAllTeams()
    {
        var teams = teamService.GetAllTeams();
        return Ok(teams);
    }
    public IActionResult Index()
    {
        var teams = teamService.GetAllTeams();
        return View(teams);  
    }

    public IActionResult Details(int id)
    {
        try
        {
            var team = teamService.GetTeamDetailsById(id);
            return View(team);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}
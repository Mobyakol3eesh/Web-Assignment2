

using Microsoft.AspNetCore.Mvc;

public class TeamController : Controller
{
    private readonly ITeamService teamService;

    public TeamController(ITeamService teamService)
    {
        this.teamService = teamService;
    }

    [HttpGet("teams/MVP/{teamId}")]
    public async Task<ActionResult<PlayerReadDto>> GetMostValuablePlayerInTeam(int teamId)
    {
        try
        {
            var mostValuablePlayer = await teamService.GetMostValuablePlayerinTeam(teamId);
            var dto = new PlayerReadDto
            {
                Id = mostValuablePlayer.Id,
                Name = mostValuablePlayer.Name,
                MarketValue = mostValuablePlayer.MarketValue,
                TeamReadDto = new TeamReadDto
                {
                    Id = mostValuablePlayer.TeamId,
                    Name = string.Empty,
                    Points = null
                }
            };
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams/{id}")]
    public async Task<ActionResult<TeamReadDto>> GetTeamDetailsById(int id)
    {
        try
        {
            var team = await teamService.GetTeamDetailsById(id);
            var dto = new TeamReadDto
            {
                Id = team.Id,
                Name = team.Name,
                Points = team.Points
            };
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams/teamplayers/{teamId}")]
    public async Task<ActionResult<IEnumerable<PlayerReadDto>>> GetAllTeamPlayers(int teamId)
    {
        try
        {
            var players = await teamService.GetALLTeamPlayers(teamId);
            var dtos = players.Select(p => new PlayerReadDto
            {
                Id = p.Id,
                Name = p.Name,
                MarketValue = p.MarketValue,
                TeamReadDto = new TeamReadDto
                {
                    Id = p.TeamId,
                    Name = string.Empty,
                    Points = null
                }
            });
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams")]
    public async Task<ActionResult<IEnumerable<TeamReadDto>>> GetAllTeams()
    {
        var teams = await teamService.GetAllTeams();
        var dtos = teams.Select(t => new TeamReadDto
        {
            Id = t.Id,
            Name = t.Name,
            Points = t.Points
        });
        return Ok(dtos);
    }
    public async Task<IActionResult> Index()
    {
        var teams = await teamService.GetAllTeams();
        return View(teams);  
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var team = await teamService.GetTeamDetailsById(id);
            return View(team);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}
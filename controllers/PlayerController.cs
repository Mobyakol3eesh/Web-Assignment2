

using Microsoft.AspNetCore.Mvc;

public class PlayerController : Controller
{
    private IPlayerService playerService;

    public PlayerController(IPlayerService playerService)
    {
        this.playerService = playerService;
    }

    [HttpGet("players")]
    public ActionResult<IEnumerable<Player>> GetAllPlayers()
    {
        var players = playerService.GetAllPlayers();
        return Ok(players);
    }

    [HttpGet("players/{id}")]
    public ActionResult<Player> GetPlayerDetailsById(int id)
    {
        try
        {
            var player = playerService.GetPlayerDetailsById(id);
            return Ok(player);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }

    }
    [HttpPost("players")]
    public ActionResult AddPlayer([FromBody] Player player)
    {
        try
        {
            playerService.AddPlayer(player.Name, player.MarketValue, player.TeamId);
            return Ok("Player added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public IActionResult Index()
    {
        var players = playerService.GetAllPlayers();
        return View(players);  
    }

    public IActionResult Details(int id)
    {
        try
        {
            var player = playerService.GetPlayerDetailsById(id);
            return View(player);
        }
        catch (Exception )
        {
            return NotFound();
        }
    }
}
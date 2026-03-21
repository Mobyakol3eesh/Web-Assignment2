

using Microsoft.AspNetCore.Mvc;

public class PlayerController : Controller
{
    private IPlayerService playerService;

    public PlayerController(IPlayerService playerService)
    {
        this.playerService = playerService;
    }

    [HttpGet("players")]
    public async Task<ActionResult<IEnumerable<PlayerReadDto>>> GetAllPlayers()
    {
        var players = await playerService.GetAllPlayers();
        return Ok(players);
    }

    [HttpGet("players/{id}")]
    public async Task<ActionResult<PlayerReadDto>> GetPlayerDetailsById(int id)
    {
        try
        {
            var player = await playerService.GetPlayerDetailsById(id);
            return Ok(player);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }

    }
    [HttpPost("players")]
    public async Task<ActionResult> AddPlayer([FromBody] CreatePlayerDto dto)
    {
        try
        {
            await playerService.AddPlayerAsync(dto);
            return Ok("Player added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> Index()
    {
        var players = await playerService.GetAllPlayers();
        return View(players);  
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var player = await playerService.GetPlayerDetailsById(id);
            return View(player);
        }
        catch (Exception )
        {
            return NotFound();
        }
    }
}
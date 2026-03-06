

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
        var dtos = players.Select(p => new PlayerReadDto
        {
            Id = p.Id,
            Name = p.Name,
            MarketValue = p.MarketValue,
            TeamId = p.TeamId
        });
        return Ok(dtos);
    }

    [HttpGet("players/{id}")]
    public async Task<ActionResult<PlayerReadDto>> GetPlayerDetailsById(int id)
    {
        try
        {
            var player = await playerService.GetPlayerDetailsById(id);
            var dto = new PlayerReadDto
            {
                Id = player.Id,
                Name = player.Name,
                MarketValue = player.MarketValue,
                TeamId = player.TeamId
            };
            return Ok(dto);
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
            await playerService.AddPlayer(dto.Name, dto.MarketValue, dto.TeamId);
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
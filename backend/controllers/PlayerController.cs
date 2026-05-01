

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
public class PlayerController : ControllerBase
{
    private IPlayerService playerService;

    public PlayerController(IPlayerService playerService)
    {
        this.playerService = playerService;
    }

    [HttpGet("players")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get all players", Description = "Returns a list of all players with their basic details and team name.")]
    public async Task<ActionResult<IEnumerable<PlayerReadDto>>> GetAllPlayers()
    {
        var players = await playerService.GetAllPlayers();
        return Ok(players);
    }

    [HttpGet("players/{id}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get player by id", Description = "Returns a single player's details by id.")]
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
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create player", Description = "Creates a new player record.")]
    public async Task<ActionResult> AddPlayer([FromBody] CreatePlayerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await playerService.AddPlayer(dto);
            return Ok("Player added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }

    [HttpPut("players/{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update player", Description = "Updates an existing player by id.")]
    public async Task<ActionResult> UpdatePlayer(int id, [FromBody] UpdatePlayerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await playerService.UpdatePlayer(id, dto);
            return Ok("Player updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }

    [HttpGet("players/player-stats")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get all player stats", Description = "Returns all player statistics entries.")]
    public async Task<ActionResult<IEnumerable<PlayerStatsReadDto>>> GetAllPlayerStats()
    {
        var playerStats = await playerService.GetAllPlayerStats();
        return Ok(playerStats);
    }

    [HttpGet("players/player-stats/{id}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get player stats by id", Description = "Returns one player stats record by id.")]
    public async Task<ActionResult<PlayerStatsReadDto>> GetPlayerStatsById(int id)
    {
        try
        {
            var playerStats = await playerService.GetPlayerStatsById(id);
            return Ok(playerStats);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("players/player-stats")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create player stats", Description = "Creates a new player stats record for a specific player and match.")]
    public async Task<ActionResult> AddPlayerStats([FromBody] CreatePlayerStatsDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await playerService.AddPlayerStats(dto);
            return Ok("Player stats added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }

    [HttpPut("players/player-stats/{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update player stats", Description = "Updates an existing player stats record by id.")]
    public async Task<ActionResult> UpdatePlayerStats(int id, [FromBody] UpdatePlayerStatsDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await playerService.UpdatePlayerStats(id, dto);
            return Ok("Player stats updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }
}
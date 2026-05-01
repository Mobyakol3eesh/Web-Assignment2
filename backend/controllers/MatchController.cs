using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
public class MatchController : ControllerBase
{
	private readonly IMatchService matchService;

	public MatchController(IMatchService matchService)
	{
		this.matchService = matchService;
	}

	[HttpGet("matches")]
	[Authorize(Roles = "User,Admin")]
	[SwaggerOperation(Summary = "Get all matches", Description = "Returns a list of all matches.")]
	public async Task<ActionResult<IEnumerable<MatchReadDto>>> GetAllMatches()
	{
		var matches = await matchService.GetAllMatches();
		return Ok(matches);
	}

	[HttpGet("matches/{id}")]
	[Authorize(Roles = "User,Admin")]
	[SwaggerOperation(Summary = "Get match by id", Description = "Returns match details by id.")]
	public async Task<ActionResult<MatchReadDto>> GetMatchDetailsById(int id)
	{
		try
		{
			var match = await matchService.GetMatchDetailsById(id);
			return Ok(match);
		}
		catch (Exception ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPost("matches")]
	[Authorize(Roles = "Admin")]
	[SwaggerOperation(Summary = "Create match", Description = "Creates a new match and related match-team entry.")]
	public async Task<ActionResult> CreateMatch([FromBody] CreateMatchDto dto)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem(ModelState);
		}

		try
		{
			await matchService.CreateMatch(dto);
			return Ok("Match created successfully.");
		}
		catch (Exception ex)
		{
			return BadRequest(ex.ToClientMessage());
		}
	}

	[HttpPut("matches/{id}")]
	[Authorize(Roles = "Admin")]
	[SwaggerOperation(Summary = "Update match", Description = "Updates an existing match and score information by id.")]
	public async Task<ActionResult> UpdateMatch(int id, [FromBody] UpdateMatchDto dto)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem(ModelState);
		}

		try
		{
			await matchService.UpdateMatch(id, dto);
			return Ok("Match updated successfully.");
		}
		catch (Exception ex)
		{
			return BadRequest(ex.ToClientMessage());
		}
	}
}




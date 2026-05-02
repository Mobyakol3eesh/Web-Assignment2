

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
public class TeamController : ControllerBase
{
        private readonly ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

    [HttpGet("teams/MVP/{teamId}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get team MVP", Description = "Returns the most valuable player in a team.")]
    public async Task<ActionResult<PlayerReadDto>> GetMostValuablePlayerInTeam(int teamId)
    {
        try
        {
            var mostValuablePlayerdto = await teamService.GetMostValuablePlayerinTeam(teamId);
            return Ok(mostValuablePlayerdto);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams/{id}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get team by id", Description = "Returns a single team with players and coach info.")]
    public async Task<ActionResult<TeamReadDto>> GetTeamDetailsById(int id)
    {
        try
        {
            var team = await teamService.GetTeamDetailsById(id);
            
            return Ok(team);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpPost("teams")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create team", Description = "Creates a new team.")]
    public async Task<ActionResult> CreateTeam([FromBody] CreateTeamDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await teamService.CreateTeam(dto);
            return Ok("Team created successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }

    [HttpPut("teams/{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update team", Description = "Updates team details by id.")]
    public async Task<ActionResult> UpdateTeam(int id, [FromBody] UpdateTeamDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await teamService.UpdateTeam(id, dto);
            return Ok("Team updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }

    [HttpGet("teams/team-matches/{teamId}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get team matches", Description = "Returns all matches where the team played as home or away.")]
    public async Task<ActionResult<IEnumerable<MatchReadDto>>> GetAllTeamMatches(int teamId)
    {
        try
        {
            var matches = await teamService.GetTeamMatches(teamId);
            return Ok(matches);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams/teamplayers/{teamId}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get team players", Description = "Returns all players for a specific team.")]
    public async Task<ActionResult<IEnumerable<PlayerReadDto>>> GetAllTeamPlayers(int teamId)
    {
        try
        {
            var players = await teamService.GetALLTeamPlayers(teamId);
            return Ok(players);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("teams")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get all teams", Description = "Returns a list of all teams.")]
    public async Task<ActionResult<IEnumerable<TeamReadDto>>> GetAllTeams()
    {
        var teams = await teamService.GetAllTeams();
       
        return Ok(teams);
    }

    [HttpDelete("teams/{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete team", Description = "Deletes a team and related data by id.")]
    public async Task<ActionResult> DeleteTeam(int id)
    {
        try
        {
            await teamService.DeleteTeam(id);
            return Ok("Team deleted successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }
}
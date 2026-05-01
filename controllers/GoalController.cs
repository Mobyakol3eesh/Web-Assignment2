using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class GoalController : Controller
{
    private readonly IGoalService goalService;

    public GoalController(IGoalService goalService)
    {
        this.goalService = goalService;
    }

    [HttpGet("goals")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get all goals", Description = "Returns all goal records.")]
    public async Task<ActionResult<IEnumerable<GoalReadDto>>> GetAllGoals()
    {
        var goals = await goalService.GetAllGoals();
        return Ok(goals);
    }

    [HttpGet("goals/{id}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get goal by id", Description = "Returns one goal record by id.")]
    public async Task<ActionResult<GoalReadDto>> GetGoalById(int id)
    {
        try
        {
            var goal = await goalService.GetGoalById(id);
            return Ok(goal);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("goals/match/{matchId}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get goals by match", Description = "Returns all goals scored in a match.")]
    public async Task<ActionResult<IEnumerable<GoalReadDto>>> GetGoalsByMatch(int matchId)
    {
        var goals = await goalService.GetGoalsByMatch(matchId);
        return Ok(goals);
    }

    [HttpGet("goals/team/{teamId}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get goals by team", Description = "Returns all goals scored by a team.")]
    public async Task<ActionResult<IEnumerable<GoalReadDto>>> GetGoalsByTeam(int teamId)
    {
        var goals = await goalService.GetGoalsByTeam(teamId);
        return Ok(goals);
    }

    [HttpPost("goals")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create goal", Description = "Creates a goal record linked to player, match, and team.")]
    public async Task<ActionResult> CreateGoal([FromBody] CreateGoalDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await goalService.CreateGoal(dto);
            return Ok("Goal created successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
public class CoachController : ControllerBase
{
    private readonly ICoachService coachService;

    public CoachController(ICoachService coachService)
    {
        this.coachService = coachService;
    }

    [HttpGet("coaches")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get all coaches", Description = "Returns a list of all coaches.")]
    public async Task<ActionResult<IEnumerable<CoachReadDto>>> GetAllCoaches()
    {
        var coaches = await coachService.GetAllCoaches();
        return Ok(coaches);
    }

    [HttpGet("coaches/{id}")]
    [Authorize(Roles = "User,Admin")]
    [SwaggerOperation(Summary = "Get coach by id", Description = "Returns coach details by id.")]
    public async Task<ActionResult<CoachReadDto>> GetCoachDetailsById(int id)
    {
        try
        {
            var coach = await coachService.GetCoachDetailsById(id);
            return Ok(coach);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("coaches")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Create coach", Description = "Creates a new coach record.")]
    public async Task<ActionResult> AddCoach([FromBody] CreateCoachDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await coachService.AddCoach(dto);
            return Ok("Coach added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }

    [HttpPut("coaches/{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update coach", Description = "Updates an existing coach by id.")]
    public async Task<ActionResult> UpdateCoach(int id, [FromBody] UpdateCoachDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await coachService.UpdateCoach(id, dto);
            return Ok("Coach updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }

    [HttpDelete("coaches/{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Delete coach", Description = "Deletes a coach by id.")]
    public async Task<ActionResult> DeleteCoach(int id)
    {
        try
        {
            await coachService.DeleteCoach(id);
            return Ok("Coach deleted successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToClientMessage());
        }
    }
}
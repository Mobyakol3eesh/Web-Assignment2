public interface IGoalService
{
    Task<IEnumerable<GoalReadDto>> GetAllGoals();
    Task<GoalReadDto> GetGoalById(int id);
    Task<IEnumerable<GoalReadDto>> GetGoalsByMatch(int matchId);
    Task<IEnumerable<GoalReadDto>> GetGoalsByTeam(int teamId);
    Task CreateGoal(CreateGoalDto dto);
}
public interface ICoachService
{
    Task<IEnumerable<CoachReadDto>> GetAllCoaches();
    Task<CoachReadDto> GetCoachDetailsById(int id);
    Task AddCoach(CreateCoachDto dto);
    Task UpdateCoach(int id, UpdateCoachDto dto);
}
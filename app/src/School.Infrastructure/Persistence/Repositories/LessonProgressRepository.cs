using Microsoft.EntityFrameworkCore;
using School.Domain.Progress;

namespace School.Infrastructure.Persistence.Repositories
{

public class LessonProgressRepository : ILessonProgressRepository
{
    private readonly SchoolDbContext _dbContext;

    public LessonProgressRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LessonProgress?> GetByIdAsync(Guid id)
    {
        return await _dbContext.LessonProgresses
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<LessonProgress?> GetByStudentAndLessonAsync(
        Guid studentId,
        Guid lessonId)
    {
        return await _dbContext.LessonProgresses
            .FirstOrDefaultAsync(p =>
                p.StudentId == studentId &&
                p.LessonId == lessonId);
    }

    public async Task<IReadOnlyList<LessonProgress>> GetByStudentAsync(Guid studentId)
    {
        return await _dbContext.LessonProgresses
            .Where(p => p.StudentId == studentId)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<LessonProgress>> GetByLessonAsync(Guid lessonId)
    {
        return await _dbContext.LessonProgresses
            .Where(p => p.LessonId == lessonId)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync();
    }

    public async Task CreateAsync(LessonProgress progress)
    {
        await _dbContext.LessonProgresses.AddAsync(progress);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(LessonProgress progress)
    {
        _dbContext.LessonProgresses.Update(progress);
        await _dbContext.SaveChangesAsync();
    }
}
}
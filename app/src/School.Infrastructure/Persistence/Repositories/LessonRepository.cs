using Microsoft.EntityFrameworkCore;
using School.Domain.Lessons;

namespace School.Infrastructure.Persistence.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly SchoolDbContext _dbContext;

    public LessonRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Lessons
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IReadOnlyList<Lesson>> GetAllAsync(Guid? subjectId = null)
    {
        var query = _dbContext.Lessons.AsQueryable();

        if (subjectId.HasValue)
        {
            query = query.Where(l => l.SubjectId == subjectId.Value);
        }

        return await query
            .OrderBy(l => l.Position)
            .ToListAsync();
    }

    public async Task CreateAsync(Lesson lesson)
    {
        await _dbContext.Lessons.AddAsync(lesson);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        _dbContext.Lessons.Update(lesson);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var lesson = await _dbContext.Lessons.FindAsync(id);
        if (lesson is not null)
        {
            _dbContext.Lessons.Remove(lesson);
            await _dbContext.SaveChangesAsync();
        }
    }
}
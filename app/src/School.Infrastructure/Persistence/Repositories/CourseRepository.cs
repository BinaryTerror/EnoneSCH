using Microsoft.EntityFrameworkCore;
using School.Domain.Courses;

namespace School.Infrastructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly SchoolDbContext _dbContext;

    public CourseRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IReadOnlyList<Course>> GetAllAsync()
    {
        return await _dbContext.Courses
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task CreateAsync(Course course)
    {
        await _dbContext.Courses.AddAsync(course);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _dbContext.Courses.Update(course);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var course = await _dbContext.Courses.FindAsync(id);
        if (course is not null)
        {
            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}
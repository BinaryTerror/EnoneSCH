using Microsoft.EntityFrameworkCore;
using School.Domain.Semesters;

namespace School.Infrastructure.Persistence.Repositories;

public class SemesterRepository : ISemesterRepository
{
    private readonly SchoolDbContext _dbContext;

    public SemesterRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Semester?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Semesters
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IReadOnlyList<Semester>> GetAllAsync(Guid? academicYearId = null)
    {
        var query = _dbContext.Semesters.AsQueryable();

        if (academicYearId.HasValue)
        {
            query = query.Where(s => s.AcademicYearId == academicYearId.Value);
        }

        return await query
            .OrderBy(s => s.SemesterNumber)
            .ToListAsync();
    }

    public async Task CreateAsync(Semester semester)
    {
        await _dbContext.Semesters.AddAsync(semester);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Semester semester)
    {
        _dbContext.Semesters.Update(semester);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var semester = await _dbContext.Semesters.FindAsync(id);
        if (semester is not null)
        {
            _dbContext.Semesters.Remove(semester);
            await _dbContext.SaveChangesAsync();
        }
    }
}
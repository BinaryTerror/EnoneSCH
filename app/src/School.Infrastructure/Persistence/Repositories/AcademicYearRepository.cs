using Microsoft.EntityFrameworkCore;
using School.Domain.AcademicYears;

namespace School.Infrastructure.Persistence.Repositories;

public class AcademicYearRepository : IAcademicYearRepository
{
    private readonly SchoolDbContext _dbContext;

    public AcademicYearRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AcademicYear?> GetByIdAsync(Guid id)
    {
        return await _dbContext.AcademicYears
            .FirstOrDefaultAsync(ay => ay.Id == id);
    }

    public async Task<IReadOnlyList<AcademicYear>> GetAllAsync(Guid? courseId = null)
    {
        var query = _dbContext.AcademicYears.AsQueryable();

        if (courseId.HasValue)
        {
            query = query.Where(ay => ay.CourseId == courseId.Value);
        }

        return await query
            .OrderBy(ay => ay.YearNumber)
            .ToListAsync();
    }

    public async Task CreateAsync(AcademicYear academicYear)
    {
        await _dbContext.AcademicYears.AddAsync(academicYear);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(AcademicYear academicYear)
    {
        _dbContext.AcademicYears.Update(academicYear);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var academicYear = await _dbContext.AcademicYears.FindAsync(id);
        if (academicYear is not null)
        {
            _dbContext.AcademicYears.Remove(academicYear);
            await _dbContext.SaveChangesAsync();
        }
    }
}
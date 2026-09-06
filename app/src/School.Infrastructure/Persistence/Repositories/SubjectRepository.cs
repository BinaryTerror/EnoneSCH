using Microsoft.EntityFrameworkCore;
using School.Domain.Subjects;

namespace School.Infrastructure.Persistence.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly SchoolDbContext _dbContext;

    public SubjectRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Subject?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Subjects
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IReadOnlyList<Subject>> GetAllAsync(Guid? semesterId = null)
    {
        var query = _dbContext.Subjects.AsQueryable();

        if (semesterId.HasValue)
        {
            query = query.Where(s => s.SemesterId == semesterId.Value);
        }

        return await query
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task CreateAsync(Subject subject)
    {
        await _dbContext.Subjects.AddAsync(subject);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subject subject)
    {
        _dbContext.Subjects.Update(subject);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var subject = await _dbContext.Subjects.FindAsync(id);
        if (subject is not null)
        {
            _dbContext.Subjects.Remove(subject);
            await _dbContext.SaveChangesAsync();
        }
    }
}
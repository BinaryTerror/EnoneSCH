using Microsoft.EntityFrameworkCore;
using School.Domain.Videos;

namespace School.Infrastructure.Persistence.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly SchoolDbContext _dbContext;

    public VideoRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Video?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Videos
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Video?> GetByLessonIdAsync(Guid lessonId)
    {
        return await _dbContext.Videos
            .Where(v => v.LessonId == lessonId && v.Status == VideoStatus.Ready)
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Video>> GetAllByLessonIdAsync(Guid lessonId)
    {
        return await _dbContext.Videos
            .Where(v => v.LessonId == lessonId)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }

    public async Task CreateAsync(Video video)
    {
        await _dbContext.Videos.AddAsync(video);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var video = await _dbContext.Videos.FindAsync(id);
        if (video is not null)
        {
            _dbContext.Videos.Remove(video);
            await _dbContext.SaveChangesAsync();
        }
    }
}
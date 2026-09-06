namespace School.Domain.Videos;

public interface IVideoRepository
{
    Task<Video?> GetByIdAsync(Guid id);
    Task<Video?> GetByLessonIdAsync(Guid lessonId);
    Task<IReadOnlyList<Video>> GetAllByLessonIdAsync(Guid lessonId);
    Task CreateAsync(Video video);
    Task DeleteAsync(Guid id);
}
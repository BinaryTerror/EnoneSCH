using School.Domain.Videos;

namespace School.Application.Videos.GetById;

public class GetVideoService
{
    private readonly IVideoRepository _repository;

    public GetVideoService(IVideoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Video?> GetByIdAsync(Guid videoId)
    {
        return await _repository.GetByIdAsync(videoId);
    }

    public async Task<Video?> GetByLessonIdAsync(Guid lessonId)
    {
        return await _repository.GetByLessonIdAsync(lessonId);
    }
}
using School.Domain.Videos;

namespace School.Application.Videos.Create;

public class CreateVideoService
{
    private readonly IVideoRepository _repository;

    public CreateVideoService(IVideoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(Guid lessonId, CreateVideoRequest request)
    {
        var video = new Video(
            lessonId,
            request.StorageKey,
            request.DurationSeconds
        );

        await _repository.CreateAsync(video);

        return video.Id;
    }
}
using School.Domain.Videos;
using School.Infrastructure.Storage;

namespace School.Application.Videos.Delete;

public class DeleteVideoService
{
    private readonly IVideoRepository _videoRepository;
    private readonly IObjectStorage _storage;

    public DeleteVideoService(
        IVideoRepository videoRepository,
        IObjectStorage storage)
    {
        _videoRepository = videoRepository;
        _storage = storage;
    }

    public async Task DeleteAsync(Guid videoId)
    {
        var video = await _videoRepository.GetByIdAsync(videoId);

        if (video is not null)
        {
            await _storage.DeleteAsync(video.StorageKey);
            await _videoRepository.DeleteAsync(videoId);
        }
    }
}
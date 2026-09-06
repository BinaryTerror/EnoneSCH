using School.Domain.Videos;
using School.Infrastructure.Storage;

namespace School.Application.Videos.Upload;

public class UploadVideoService
{
    private readonly IVideoRepository _videoRepository;
    private readonly IObjectStorage _storage;

    public UploadVideoService(
        IVideoRepository videoRepository,
        IObjectStorage storage)
    {
        _videoRepository = videoRepository;
        _storage = storage;
    }

    public async Task<Guid> UploadAsync(Guid lessonId, UploadVideoRequest request)
    {
        var extension = Path.GetExtension(request.FileName);
        var storageKey = $"lessons/{lessonId}/videos/{Guid.NewGuid()}{extension}";

        await _storage.UploadAsync(
            storageKey,
            request.FileStream,
            request.ContentType);

        var video = new Video(
            lessonId,
            storageKey,
            0);

        // Local storage is immediately available; in production the video
        // would be transcoded first and only then marked as Ready.
        video.MarkAsReady(null);

        await _videoRepository.CreateAsync(video);

        return video.Id;
    }
}
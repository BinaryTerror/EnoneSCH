using School.Domain.Lessons;

namespace School.Domain.Videos;

public enum VideoStatus
{
    Processing = 0,
    Ready = 1,
    Failed = 2
}

public class Video
{
    public Guid Id { get; private set; }
    public Guid LessonId { get; private set; }
    public string StorageKey { get; private set; } = string.Empty;
    public int DurationSeconds { get; private set; }
    public string? ThumbnailUrl { get; private set; }
    public VideoStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation property
    public Lesson Lesson { get; private set; } = null!;

    private Video() { }

    public Video(Guid lessonId, string storageKey, int durationSeconds)
    {
        Id = Guid.NewGuid();
        LessonId = lessonId;
        StorageKey = storageKey;
        DurationSeconds = durationSeconds;
        ThumbnailUrl = null;
        Status = VideoStatus.Processing;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsReady(string? thumbnailUrl)
    {
        Status = VideoStatus.Ready;
        ThumbnailUrl = thumbnailUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = VideoStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }
}
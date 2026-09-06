namespace School.Application.Videos.Create;

public record CreateVideoRequest(
    string StorageKey,
    int DurationSeconds
);
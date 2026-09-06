namespace School.Infrastructure.Storage;

public interface IObjectStorage
{
    Task<string> UploadAsync(
        string key,
        Stream stream,
        string contentType);

    Task<Stream> DownloadAsync(string key);

    Task<string> GetPresignedUrlAsync(
        string key,
        TimeSpan expiry);

    Task DeleteAsync(string key);
}
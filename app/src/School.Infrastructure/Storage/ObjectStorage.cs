namespace School.Infrastructure.Storage;

public class ObjectStorage : IObjectStorage
{
    private readonly string _basePath;

    public ObjectStorage(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(
        string key,
        Stream stream,
        string contentType)
    {
        var filePath = GetFullPath(key);
        var directory = Path.GetDirectoryName(filePath)!;
        Directory.CreateDirectory(directory);

        await using var fileStream = File.Create(filePath);
        await stream.CopyToAsync(fileStream);

        return key;
    }

    public Task<Stream> DownloadAsync(string key)
    {
        var filePath = GetFullPath(key);
        var stream = File.OpenRead(filePath);
        return Task.FromResult<Stream>(stream);
    }

    public Task<string> GetPresignedUrlAsync(
        string key,
        TimeSpan expiry)
    {
        // For local storage, return the file path
        // For S3/CDN, generate a presigned URL
        var filePath = GetFullPath(key);
        return Task.FromResult(filePath);
    }

    public Task DeleteAsync(string key)
    {
        var filePath = GetFullPath(key);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }

    private string GetFullPath(string key)
    {
        return Path.Combine(_basePath, key);
    }
}
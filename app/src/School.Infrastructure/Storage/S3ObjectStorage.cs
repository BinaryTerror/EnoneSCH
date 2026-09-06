using Amazon.S3;
using Amazon.S3.Model;

namespace School.Infrastructure.Storage;

/// <summary>
/// S3-compatible object storage (Cloudflare R2, MinIO, AWS S3...).
/// </summary>
public class S3ObjectStorage : IObjectStorage
{
    private readonly string _bucket;
    private readonly AmazonS3Client _client;

    public S3ObjectStorage(
        string endpoint,
        string accessKey,
        string secretKey,
        string bucket)
    {
        _bucket = bucket;
        _client = new AmazonS3Client(
            accessKey,
            secretKey,
            new AmazonS3Config
            {
                ServiceURL = endpoint,
                ForcePathStyle = true,
                AuthenticationRegion = "auto"
            });
    }

    public async Task<string> UploadAsync(
        string key,
        Stream stream,
        string contentType)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = key,
            InputStream = stream,
            ContentType = contentType
        };

        await _client.PutObjectAsync(request);
        return key;
    }

    public async Task<Stream> DownloadAsync(string key)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucket,
            Key = key
        };

        var response = await _client.GetObjectAsync(request);
        return response.ResponseStream;
    }

    public Task<string> GetPresignedUrlAsync(
        string key,
        TimeSpan expiry)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucket,
            Key = key,
            Expires = DateTime.UtcNow.Add(expiry)
        };

        return Task.FromResult(_client.GetPreSignedURL(request));
    }

    public async Task DeleteAsync(string key)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucket,
            Key = key
        };

        await _client.DeleteObjectAsync(request);
    }
}
namespace School.Application.Videos.Upload;

public record UploadVideoRequest(
    Stream FileStream,
    string FileName,
    string ContentType
);
using RSAM.Application.File.Common;

namespace RSAM.Application.File;

public interface IFileStorage
{
    Task<FileStorageResult> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default);
}

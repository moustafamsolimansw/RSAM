using RSAM.Application.File;
using RSAM.Application.File.Common;

namespace RSAM.Infrastructure.File;

public class FileStorage : IFileStorage
{
    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileStorageResult> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

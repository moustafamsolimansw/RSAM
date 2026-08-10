namespace RSAM.Application.File.Common;

public record FileStorageResult(bool IsSuccess, string? ErrorMessage = null);

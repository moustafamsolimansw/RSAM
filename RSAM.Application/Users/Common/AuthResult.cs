namespace RSAM.Application.Users.Common;

public record AuthResult
(
  string AccessToken,
  string RefreshToken,
  DateTime RefreshTokenExpiration
);
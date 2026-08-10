namespace RSAM.Contracts.Auth.Login;

public record LoginRequest(string UsernameOrEmail, string Password);

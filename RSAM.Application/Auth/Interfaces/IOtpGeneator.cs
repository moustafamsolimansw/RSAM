namespace RSAM.Application.Auth.Interfaces;

public interface IOtpGeneator
{
    Task<string> GenerateOtpAsync();
}

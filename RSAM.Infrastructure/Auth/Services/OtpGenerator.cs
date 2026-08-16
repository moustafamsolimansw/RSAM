using RSAM.Application.Auth.Interfaces;
using System.Security.Cryptography;

namespace RSAM.Infrastructure.Auth.Services;

public sealed class OtpGenerator : IOtpGeneator
{
    public async Task<string> GenerateOtpAsync()
    {
        await Task.CompletedTask;
        string result = string.Empty;
        var random =  RandomNumberGenerator.GetInt32(100000,1000000);
        return random.ToString();
    }
}

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RSAM.Application.Auth.Interfaces;
using RSAM.Application.Time;
using RSAM.Domain.UserAR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RSAM.Infrastructure.Auth.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;
    
    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtSettingsOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtSettingsOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var secret = !string.IsNullOrEmpty(_jwtSettings.Secret) 
            ? _jwtSettings.Secret 
            : "your_secret_key_here"; // Fallback just in case

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), 
            SecurityAlgorithms.HmacSha256);
            
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.GivenName, user.PersonInfo.FirstNameInEnglish),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.PersonInfo.LastNameInEnglish),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

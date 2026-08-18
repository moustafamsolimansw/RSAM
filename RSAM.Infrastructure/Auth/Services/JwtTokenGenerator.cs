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
            : "your_secret_key_here";
        var issuer = !string.IsNullOrEmpty(_jwtSettings.Issuer) ? _jwtSettings.Issuer : "RSAM";
        var audience = !string.IsNullOrEmpty(_jwtSettings.Audience) ? _jwtSettings.Audience : "RSAM";
        var expirationMinutes = _jwtSettings.AccessTokenExpiration > 0 ? _jwtSettings.AccessTokenExpiration : 60;

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
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

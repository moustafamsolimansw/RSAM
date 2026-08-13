using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using RSAM.Application.Auth.Interfaces;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Common;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Domain.ValueObjects;

namespace RSAM.Application.Users.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IReadRepository<User, UserId> _userRepository;
    //private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<LoginQueryHandler> _logger;
    public LoginQueryHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IReadRepository<User, UserId> userRepository,
        ILogger<LoginQueryHandler> logger
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _logger = logger;
    }
    public async Task<ErrorOr<AuthResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        // Check if the user exists
        var existingUser = await _userRepository.GetFirstOrDefaultAsync(
            u => u.EmailAddress.Value == query.EmailOrUsername || u.Username == query.EmailOrUsername || u.PhoneNumber.Value == query.EmailOrUsername,
            true,
            cancellationToken,
            u => u.UserCredentials
        );
        if(existingUser is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }
        if(existingUser.UserCredentials.FirstOrDefault(c=>!c.IsDeleted) is null)
        {
            return Error.NotFound("UserCredentials.NotFound", "User credentials not found.");
        }

        // Generate the JWT token

        // Update the refresh token in the database

        // return the result
    }
}

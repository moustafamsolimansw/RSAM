using ErrorOr;
using MediatR;
using RSAM.Application.Users.Common;

namespace RSAM.Application.Users.Queries;

public record LoginQuery
(
    string EmailOrUsername,
    string Password
) : IRequest<ErrorOr<AuthResult>>;

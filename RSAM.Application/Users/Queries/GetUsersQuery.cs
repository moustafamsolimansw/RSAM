using ErrorOr;
using MediatR;
using RSAM.Application.Users.Common;

namespace RSAM.Application.Users.Queries;

public record GetUsersQuery(int skip, int take) : IRequest<ErrorOr<List<UserDto>>>;
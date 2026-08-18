using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Common;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;
using Microsoft.EntityFrameworkCore;
namespace RSAM.Application.Users.Queries;

public  class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ErrorOr<List<UserDto>>>
{
    private readonly ILogger<GetUsersQueryHandler> _logger;
    private readonly IReadRepository<User, UserId> _usersReadRepo;

    public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, IReadRepository<User, UserId> usersReadRepo)
    {
        _logger = logger;
        _usersReadRepo = usersReadRepo;
    }

    public async Task<ErrorOr<List<UserDto>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var querable = _usersReadRepo.AsQueryable();
        querable = querable.Take(query.take).Skip(query.skip);
        var users = await querable.ToListAsync();
        if (users is null)
        {
            _logger.LogError("Users list is null");
            return Error.Failure("Users list is null");
        }
        List<UserDto> result = new List<UserDto>();
        foreach (var user in users)
            result.Add(new UserDto(user.Id.Value, user.EmailAddress.Value, user.PersonInfo.FirstNameInEnglish, user.PersonInfo.LastNameInEnglish, user.Username, user.PhoneNumber.Value));
        return result;
    }

    
}

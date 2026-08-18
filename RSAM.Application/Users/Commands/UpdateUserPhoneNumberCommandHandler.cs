using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Common;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Domain.ValueObjects;

namespace RSAM.Application.Users.Commands;

public class UpdateUserPhoneNumberCommandHandler : IRequestHandler<UpdateUserPhoneNumberCommand, ErrorOr<UserDto>>
{
    private readonly IReadRepository<User, UserId> _usersReadRepository;
    private readonly IWriteUserRepositry _writeUserRepositry;
    private readonly ILogger<UpdateUserPhoneNumberCommandHandler> _logger;
    private readonly IWriteUnitOfWork _writeUnitOfWork;

    public UpdateUserPhoneNumberCommandHandler(IReadRepository<User, UserId> usersReadRepository, IWriteUserRepositry writeUserRepositry, ILogger<UpdateUserPhoneNumberCommandHandler> logger, IWriteUnitOfWork writeUnitOfWork)
    {
        _usersReadRepository = usersReadRepository;
        _writeUserRepositry = writeUserRepositry;
        _logger = logger;
        _writeUnitOfWork = writeUnitOfWork;
    }

    public async Task<ErrorOr<UserDto>> Handle(UpdateUserPhoneNumberCommand command, CancellationToken cancellationToken)
    {
        try 
        {
            UserId userId = UserId.Create(command.UserId);
            User? user = await _usersReadRepository.GetByIdAsync(userId, false, cancellationToken);
            if (user is null)
            {
                _logger.LogError("Can NOT get this user. The user is not found");
                return Error.Failure("Can NOT get this user. The user is not found");
            }
            PhoneNumber phoneNumber = PhoneNumber.Create(command.PhoneNumber);
            user.UpdatePhoneNumber(phoneNumber);
            await _writeUnitOfWork.SaveChangesAsync(cancellationToken);
            return new UserDto(user.Id.Value, user.EmailAddress.Value, user.PersonInfo.FirstNameInEnglish, user.PersonInfo.LastNameInEnglish, user.Username, user.PhoneNumber.Value);
        } 
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return Error.Failure(ex.Message);
        }

    }
}

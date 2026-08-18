using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using RSAM.Application.Helpers;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Common;
using RSAM.Domain.Enums;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Domain.ValueObjects;

namespace RSAM.Application.Users.Commands;

public sealed class UpdatePersonalInfoCommandHandler : IRequestHandler<UpdatePersonalInfoCommand, ErrorOr<UserDto>>
{
    private readonly ILogger<UpdatePersonalInfoCommandHandler> _logger;
    private readonly IReadRepository<User, UserId> _userReadRepository;
    private readonly IWriteUnitOfWork _writeUnitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdatePersonalInfoCommandHandler(ILogger<UpdatePersonalInfoCommandHandler> logger, IReadRepository<User, UserId> userReadRepository, IWriteUnitOfWork writeUnitOfWork, ICurrentUser currentUser)
    {
        _logger = logger;
        _userReadRepository = userReadRepository;
        _writeUnitOfWork = writeUnitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ErrorOr<UserDto>> Handle(UpdatePersonalInfoCommand command, CancellationToken cancellationToken)
    {
        var gender = Enum.TryParse<Gender>(command.Gender, out Gender genderValue);
        UserId userId = UserId.Create(command.UserId);
        User? user = await _userReadRepository.GetFirstOrDefaultAsync(x => x.Id == userId, true, cancellationToken, x =>x.PersonInfo);
        if (user is null)
        {
            _logger.LogError("Can NOT find this user to update the personal info.");
            return Error.Failure("Can NOT find this user to update the personal info.");
        }
        Address address = Address.Create(command.Address.Street, command.Address.City, command.Address.State, command.Address.Country);
        PersonInfo personInfo = PersonInfo.Create(command.FirstNameInEnglish, command.MiddleNameInEnglish, command.LastNameInEnglish,
            command.FirstNameInArabic, command.MiddleNameInArabic, command.LastNameInArabic, command.DateOfBirth, genderValue, address);
        user.UpdatePersonalInfo(personInfo, _currentUser?.UserId?.ToString() ?? "unknown");
        await _writeUnitOfWork.SaveChangesAsync(cancellationToken);
        return new UserDto(user.Id.Value, user.EmailAddress.Value, user.PersonInfo.FirstNameInEnglish, user.PersonInfo.LastNameInEnglish, user.Username, user.PhoneNumber.Value);

    }
}

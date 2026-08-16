using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Common;
using RSAM.Domain.Enums;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Domain.ValueObjects;

namespace RSAM.Application.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<UserDto>>
{
    private readonly IWriteRepository<User, UserId> _writeUserRepository;
    private readonly IReadRepository<User, UserId> _readUserRepository;
    private readonly IWriteUnitOfWork _writeUnitOfWork;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(IWriteRepository<User, UserId> writeUserRepository, IReadRepository<User, UserId> readUserRepository, IWriteUnitOfWork writeUnitOfWork, ILogger<CreateUserCommandHandler> logger)
    {
        _writeUserRepository = writeUserRepository;
        _readUserRepository = readUserRepository;
        _writeUnitOfWork = writeUnitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<UserDto>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Check if the user already exists by email
        var existingUser = await _readUserRepository.GetFirstOrDefaultAsync(u => u.EmailAddress.Value == command.email);

        if (existingUser is not null)
        {
            _logger.LogWarning("User with email {Email} already exists.", command.email);
            return Error.Validation("User already exists", "USER_EXISTS");
        }
        if (!Enum.TryParse<Gender>(command.gender, true, out var gender))
        {
            _logger.LogWarning("Invalid gender value provided: {Gender}", command.gender);
            return Error.Validation("Invalid gender value", "INVALID_GENDER");
        }
        // Create the new user
        var user = User.Create(
            command.username,
            EmailAddress.Create(command.email),
            PersonInfo.Create(
                command.firstNameInEnglish,
                command.middleNameInEnglish,
                command.lastNameInEnglish,
                command.firstNameInArabic,
                command.middleNameInArabic,
                command.lastNameInArabic,
                command.dateOfBirth,
                gender,
                Address.Create(
                    command.street,
                    command.city,
                    command.state,
                    command.country
                ) 
            ),
            PhoneNumber.Create(command.phoneNumber)
        );

        await _writeUserRepository.AddAsync(user, cancellationToken);
        await _writeUnitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User created successfully with ID {UserId}.", user.Id);

        return new UserDto(
            user.Id.Value,
            user.EmailAddress.Value,
            user.PersonInfo.FirstNameInEnglish,
            user.PersonInfo.LastNameInEnglish
        );
    }
}

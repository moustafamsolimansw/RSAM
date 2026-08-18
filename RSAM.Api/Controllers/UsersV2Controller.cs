using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Commands;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Api.Controllers;

    [Asp.Versioning.ApiVersion(1.1)]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersV2Controller : ControllerBase
    {
    private readonly IMediator _mediator;
    private readonly IReadRepository<User, UserId> _userRepository;
    private readonly ILogger<UsersV2Controller> _logger;

    public UsersV2Controller(IMediator mediator, IReadRepository<User, UserId> userRepository, ILogger<UsersV2Controller> logger)
    {
        _mediator = mediator;
        _userRepository = userRepository;
        _logger = logger;
    }
    [HttpPost()]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Commands;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReadRepository<User, UserId> _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, IReadRepository<User, UserId> userRepository, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _logger = logger;
        }
        [HttpPost("create")]
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
}

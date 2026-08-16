using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Commands;
using RSAM.Application.Users.Queries;
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
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOTP(GetOtpCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting otp");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your getting otp request");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reseting password");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your reset password request");
            }
        }
    }
}

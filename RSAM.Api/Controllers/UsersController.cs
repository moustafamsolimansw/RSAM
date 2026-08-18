using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSAM.Application.Repositories;
using RSAM.Application.Users.Commands;
using RSAM.Application.Users.Queries;
using RSAM.Contracts.Users.Commands;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Api.Controllers
{
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/users")]
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
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetUsers([FromQuery]GetUsersQuery query)
        {
            try 
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cant get users list");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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

        #region UPDATE
        [Authorize]
        [HttpPatch("update-phone-number/{userId}")]
        public async Task<IActionResult> UpdatePhoneNumber(string userId, UpdateUserPhoneNumberRequest request)
        {
            try 
            {
                var userIdAsGuid = Guid.TryParse(userId, out Guid userIdCasted);
                var command = new UpdateUserPhoneNumberCommand(userIdCasted, request.PhoneNumber);
                var result = await _mediator.Send(command);
                return Ok(result);
            } 
            catch (Exception ex) 
            { 
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("update-personal-info/{userId}")]
        public async Task<IActionResult> UpdatePersonalInfo(string userId, UpdateUserPersonalInfoRequest request)
        {
            try
            {
                var userIdAsGuid = Guid.TryParse(userId, out Guid userIdCasted);

                AddressDto address = new AddressDto(request.Street, request.City, request.State, request.Country);
                var command = new UpdatePersonalInfoCommand(userIdCasted, request.FirstNameInEnglish,
                    request.MiddleNameInEnglish, request.LastNameInEnglish, request.FirstNameInArabic,
                    request.MiddleNameInArabic, request.LastNameInArabic, request.DateOfBirth, request.Gender, address);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}

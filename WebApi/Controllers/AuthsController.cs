using Business.Abstract;
using Core.Utilities.Messages;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthsController : BaseApiController
    {
        private readonly IAuthService _authService;
        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous,HttpPost("login")]
        //[Produces("application/json", "application/xml")]
        [Produces("application/json", "application/xml", Type = typeof(List<string>))]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);

            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            
            return BadRequest(result.Message);
        }

        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var user = _authService.GetCurrentUser();
            if(user.Success)
            {
                return Ok(user.Data);
            }
            return BadRequest(BusinessMessages.UserNotFound);
        }

        [AllowAnonymous,HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var isUserExists = _authService.IsUserExists(userForRegisterDto.Email);
            if (isUserExists.Success)
            {
                return BadRequest(isUserExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }
    }
}

using CourseApi.Entities;
using CourseApi.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            if (model.Password != model.ConfirmPassword)
                return BadRequest();

            if (await _userManager.Users.AnyAsync(u => u.UserName == model.UserName))
                return BadRequest();

            var user = model.Adapt<User>();

            await _userManager.CreateAsync(user);

            _logger.LogInformation("User saved to database with id {0}", user.Id);

            await _signInManager.SignInAsync(user, isPersistent: true);

            _logger.LogInformation("User registred {0}", user.Id);

            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _userManager.Users.AnyAsync(user => user.UserName == model.UserName))
                return NotFound();

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, false);

            if(!result.Succeeded)
                return Ok("Successful");

            return Ok(model);
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> Profile(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.UserName != username)
                return NotFound();
           
            var UserDto = user.Adapt<UserDto>();

            _logger.LogInformation("User profile with id {0}", user.Id);

            return Ok(UserDto);

        }
    }
}

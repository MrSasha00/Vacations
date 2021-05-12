using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationsAPI.Models.User;
using System.ComponentModel.DataAnnotations;
using VacationsAPI.Models.Worker;

namespace VacationsAPI.Controllers
{
    public class UserDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
    
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MongoUserRepository _userRepository;
        private readonly MongoWorkerRepository _workerRepository;
        public AuthController(MongoUserRepository userRepository, MongoWorkerRepository workerRepository)
        {
            _userRepository = userRepository;
            _workerRepository = workerRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var newUser = new UserEntity(user.Login, user.Password);
            await _userRepository.Insert(newUser);
            return Created("api/[controller]" + newUser.Login, newUser.Password);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO logUser)
        {
            if (logUser == null)
            {
                return BadRequest();
            }
            var user = await _userRepository.GetByLogin(logUser.Login);
            if (user == null)
            {
                return NotFound(logUser.Login);
            }

            if (string.Compare(logUser.Password, user.Password, StringComparison.Ordinal) != 0)
            {
                return NotFound(logUser.Password);
            }
            //TODO: аутентификация
            return Ok(logUser);
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //TODO: logout
            return Ok();
        }

        [HttpDelete("{login}")]

        public async Task<IActionResult> Delete(string login)
        {
            await _userRepository.Delete(login);
            return Ok();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Core.Resource;
using Core.Services;
using Data.DataRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.date;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;


        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger, IConfiguration config )
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _config = config;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResource?>> GetByIdUser(int id)
        {
            var user = await _userService.GetById(id);
            return Ok(_mapper.Map<UserResource>(user));
        }
        [HttpGet]
        public async Task<ActionResult<List<UserResource>>> GetAll()
        {

            List<User> users=await  _userService.GetAll();
            return Ok(_mapper.Map<List<UserResource>>(users));
        }
       
        [HttpPost]
        private async Task<ActionResult<int>> Add(UserResource u)
        {
            var user = _mapper.Map<User>(u);
            return Ok(await _userService.Add(user));
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteById(int id)
        {
            return Ok(await _userService.DeleteById(id));
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UserResource>> Update(UserResource u)
        {
            var updatedUser = _mapper.Map<User>(u);
            if (updatedUser == null)
            {
                return NotFound($"User with ID {u.Id} not found.");
            }
            var newUser = await _userService.Update(updatedUser);
            return Ok(_mapper.Map<UserResource>(newUser));

        }
        
        [HttpPost("Register")]
        public async Task<ActionResult<AuthResult>> Register([FromBody] UserResource userResource)
        {
            try
            {
                var user = _mapper.Map<User>(userResource);
                int id = await _userService.Register(user);
                var token = GenerateJwtToken(id, user.Email);
                return Ok(new AuthResult { Token=token,UserId=id});
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }

        }
       
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResult>> Login([FromBody]LoginResource loginResource)
        {
            try
            {
                int userId=await _userService.Login(loginResource);
                var token = GenerateJwtToken(userId, loginResource.Email);
                return Ok(new AuthResult { Token = token, UserId = userId });
            }
            catch (KeyNotFoundException ex)
            {
                // אנגולר יקבל סטטוס 404 ויעביר להרשמה, אך הטקסט הרשמי נשאר עמום לאבטחה
                return NotFound(new { message = "שם משתמש או סיסמה שגויים" });
            }
            catch (UnauthorizedAccessException ex)
            {
                // אנגולר יקבל סטטוס 401 ויציג שגיאה רגילה
                return Unauthorized(new { message = "שם משתמש או סיסמה שגויים" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [NonAction]
        private string GenerateJwtToken(int userId,string email)
        {
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
                 new Claim(ClaimTypes.Email,email),
            };
            var key= new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
                );
            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}

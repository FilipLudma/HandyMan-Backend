using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using HandyManAPI.Helpers;
using HandyManAPI.Inputs;
using HandyManAPI.Interfaces.Repositories;
using HandyManAPI.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UsersController : Controller
    {
        private IUserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserRepository userRepository,
            IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        #region Public Methods

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _userRepository.Authenticate(userDto.EmailAddress, userDto.Password);

            if (user == null)
                return Json(Unauthorized());

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Json(new
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = Mapper.Map<UserRecord>(userDto);

            try
            {
                // save 
                _userRepository.Create(user, userDto.Password);
                return Json(Ok());
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return Json(BadRequest(ex.Message));
            }
        }

        [HttpGet]
        public JsonResult Authenticated()
        {
            return Json(Ok());
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userRepository.GetAll();
            var userDtos = Mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userRepository.GetById(id);
            var userDto = Mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = Mapper.Map<UserRecord>(userDto);
            user.Id = id;

            try
            {
                // save 
                _userRepository.Update(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userRepository.Delete(id);
            return Ok();
        }

        #endregion Public Methods
    }
}
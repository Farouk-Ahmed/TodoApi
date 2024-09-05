using Domain.Bases;
using Domain.DTO.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;
		public AccountController(UserManager<User> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}

		[HttpPost("login")]
		public async Task<ActionResult<CustomResponse<LoginDTO>>> Login([FromBody] LoginDTO userDTO)
		{
			var Result = new CustomResponse<LoginDTO>();

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(userDTO.Email);
				if (user != null && await _userManager.CheckPasswordAsync(user, userDTO.Password))
				{
					var token = GenerateJwtToken(user);
					Result.IsCompleted = true;
					Result.Data = new
					{
						token = new JwtSecurityTokenHandler().WriteToken(token),
						expiration = token.ValidTo
					};
					Result.Message = "Token created successfully";
				}
				else
				{
					Result.IsCompleted = false;
					Result.Message = "Your Not Authorized";
				}
			}
			else
			{
				Result.IsCompleted = false;
				Result.Message = "Invalid login";
				Result.Data = ModelState;
			}
			return Result;
		}


		[HttpPost("register")]
		public async Task<ActionResult<CustomResponse<RegisterDTO>>> Register([FromBody] RegisterDTO registerModel)
		{
			var customResult = new CustomResponse<RegisterDTO>();
			if (ModelState.IsValid)
			{
				var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
				if (existingUser != null)
				{
					customResult.IsCompleted = false;
					customResult.Message = "Email already Exist";
					return customResult;
				}

				var existingUserByUserName = await _userManager.FindByNameAsync(registerModel.UserName);
				if (existingUserByUserName != null)
				{
					customResult.IsCompleted = false;
					customResult.Message = "Username already Exist";
					return customResult;
				}
				var highestUserId = await _userManager.Users.AnyAsync() ? await _userManager.Users.MaxAsync(u => u.UserId) : 1;

				var user = new User
				{
					UserName = registerModel.UserName,
					Email = registerModel.Email,
					UserId = highestUserId
				};

				var createResult = await _userManager.CreateAsync(user, registerModel.Password);

				if (createResult.Succeeded)
				{
					customResult.IsCompleted = true;
					customResult.Message = "Account created succesfully";
					customResult.Data = createResult;

				}
				else
				{
					customResult.IsCompleted = false;
					customResult.Message = "Account creation failed";
					customResult.Data = ModelState;
				}
			}
			else
			{
				customResult.IsCompleted = false;
				customResult.Message = "Invalid Register Data";
				customResult.Data = ModelState;
			}
			return customResult;
		}




		private JwtSecurityToken GenerateJwtToken(User user)
		{
			var claims = new List<Claim>
		{
			new Claim("userId", user.UserId.ToString()),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:secretKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			return new JwtSecurityToken(
				issuer: _configuration["JWT:issuer"],
				audience: _configuration["JWT:audience"],
				expires: DateTime.Now.AddHours(8),
				claims: claims,
				signingCredentials: creds
			);
		}


	}
}

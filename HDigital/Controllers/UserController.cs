using HDigital.Context;
using HDigital.Helper;
using HDigital.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using HDigital.Models.Dto;
using HDigital.UtilityService;
using System.Diagnostics;


namespace HDigital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly AppDbContext _hubContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserController> _logger;
        public UserController(AppDbContext appDbContext, IConfiguration configuration, IEmailService emailService, ILogger<UserController> logger)
        {
            _hubContext = appDbContext;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            
            var user = await _hubContext.User.FirstOrDefaultAsync(x => x.Username.Equals(userObj.Username));
         
            if (user == null)
                return NotFound(new { Message = "Utilizatorul nu a fost gasit!" });

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = " Parola incorecta " });
            }

            user.Token = CreateJwt(user);
            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            await _hubContext.SaveChangesAsync();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                UserId = user.Id,

            });

        }
        
        [HttpPost("inregistrare")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();
            //checked username
            if (await CheckUserNameExistAsync(userObj.Username))
                return BadRequest(new { Message = "Numele de utilizator este deja folosit!" });

            //check email 
            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email-ul este deja utilizat!" });


         

            //check password strentgth
            var pass = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass.ToString() }); 

            

            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            await _hubContext.User.AddAsync(userObj);
            await _hubContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Utilizator inregistrat!"
            });
        }

        private Task<bool> CheckUserNameExistAsync(string username)
            => _hubContext.User.AnyAsync(x => x.Username == username);

        private Task<bool> CheckEmailExistAsync(string email)
            => _hubContext.User.AnyAsync(x => x.Email == email);

           private string CheckPasswordStrength(string password)
           {
               StringBuilder sb = new StringBuilder();
               if (password.Length < 8)
                   sb.Append("Parola trebuie sa aiba minim 8 caractere" + Environment.NewLine);
               if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                   && Regex.IsMatch(password, "[0-9]")))
                   sb.Append("Parola trebuie sa fie alfanumerica" + Environment.NewLine);
               if (!Regex.IsMatch(password, "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")) 
                   sb.Append("Parola trebuie sa contina caractere speciale" + Environment.NewLine);
               return sb.ToString();
           }  
       


        private string CreateJwt(User user) //CreareToken
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret......"); 
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.Username} "),
                new Claim("userId", user.Id.ToString())

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _hubContext.User
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysecret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _hubContext.User.ToListAsync());
        }

        [HttpPost("refresh1")]
        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _hubContext.User.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Request");
            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _hubContext.SaveChangesAsync();
            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }
      
        [HttpPost("send-reset-email1/{email}")]
        public async Task<IActionResult> SendEmail(string email) 
        {
            var user = await _hubContext.User.FirstOrDefaultAsync(a => a.Email == email);
            if(user is null)
            {
                return NotFound(new
                {
                    StatausCode = 404,
                    Message = "Email-ul nu exista"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPaswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Modificarea parolei", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _hubContext.Entry(user).State = EntityState.Modified;
            await _hubContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Email trimis!"
            });
        }
      
        [HttpPost("reset-password1")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken =  resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _hubContext.User.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatausCode = 404,
                    Message = "Email-ul nu exista"
                });
            }
            var tokenCode = user.ResetPaswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;
            if(tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Link-ul de modificare a parolei este nevalid"
                });
            }
            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _hubContext.Entry(user).State = EntityState.Modified;
            await _hubContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Resetarea parolei cu succes!"
            });
        }

        
    }
}

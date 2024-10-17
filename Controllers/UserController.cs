using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DataConnector;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QuanLySanPhamContext _context;
        private readonly IConfiguration _configuration;

        public UserController(QuanLySanPhamContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            var existUser = await _context.Users.Where(p => p.UserName == username).FirstOrDefaultAsync();
            if (existUser != null)
            {
                return BadRequest("This username is already exist");
            }
            var user = new User();
            user.id = Guid.NewGuid();
            user.UserName = username;
            user.Password = password;
            _context.Users.Add(user);
            _context.SaveChanges();
            var token = GenerateToken(user);
            return Ok(new APIRespond() { UserId = user.id, AccessToken = token });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.Where(p => p.UserName == username && p.Password == password).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Wrong username or password");
            }
            var token = GenerateToken(user);
            return Ok(new APIRespond() { UserId = user.id, AccessToken = token });
        }

        private string GenerateToken(User user)
        {
            // phát sinh token và trả về cho người dùng sau khi đăng nhập thành công
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["AppSettings:SecretKey"];
            var secterKeyByte = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // nội dung của token   
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserID", user.id.ToString()),
                    // role
                }),
                // thời gian sống của token
                Expires = DateTime.UtcNow.AddHours(1),
                // ký vào token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secterKeyByte), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var accessToken = jwtTokenHandler.WriteToken(token);
            return accessToken;

        }
    }
    public class APIRespond
    {
        public Guid UserId { get; set; }
        public string AccessToken { get; set; }
    }
}
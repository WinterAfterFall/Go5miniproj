using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return Unauthorized(new { message = "ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("MySuperSecretKey12345678901234567890");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.RoleType.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token), user = MapToDTO(user) });
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdFromToken)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userIdFromToken);
            return (user == null) ? NotFound() : Ok(MapToDTO(user));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.OrderByDescending(u => u.CreateDate).ToListAsync();
            return Ok(users.Select(u => MapToDTO(u)));
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            user.CreateDate = DateTime.Now;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, MapToDTO(user));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return (user == null) ? NotFound() : Ok(MapToDTO(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static UserDTO MapToDTO(User user)
        {
            // ดักจับกรณีข้อมูลใน DB มีค่าที่ไม่ตรงกับ Enum (เช่น คำว่า 'string')
            string roleLabel = "Employee";

            if (Enum.IsDefined(typeof(RoleTypeEnum), user.RoleType))
            {
                roleLabel = user.RoleType switch
                {
                    RoleTypeEnum.SuperAdmin => "Super Admin",
                    RoleTypeEnum.HRAdmin => "HR Admin",
                    _ => user.RoleType.ToString()
                };
            }

            return new UserDTO
            {
                Id = user.Id,
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                RoleType = roleLabel,
                CreateDate = user.CreateDate
            };
        }
    }

    public class LoginRequest { public string Username { get; set; } = ""; public string Password { get; set; } = ""; }
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string RoleType { get; set; } = "";
        public DateTime CreateDate { get; set; }
    }
}
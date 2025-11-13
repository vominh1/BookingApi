using BookingApi.DTOs;
using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BookingDBContext _context;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IConfiguration _config;
   
    public AuthController(IConfiguration config, BookingDBContext context, IPasswordHasher<User> hasher)
    {
        _config = config;
        _context = context;
        _hasher = hasher;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);

        if (user == null)
            return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash!, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });

        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
        var token = GenerateJwtToken(user, roles);

        return Ok(new
        {
            token,
            user = new { user.UserId, user.Username, user.Email, roles }
        });
    }

    private string GenerateJwtToken(User user, List<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim("userId", user.UserId.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

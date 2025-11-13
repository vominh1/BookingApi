using BookingApi.DTOs;
using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
    {
    private readonly BookingDBContext _context;
    private readonly IPasswordHasher<User> _hasher;

    public UsersController(BookingDBContext context, IPasswordHasher<User> hasher)
    {
        _context = context;
        _hasher = hasher;
    }
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(u=>u.UserRoles).ThenInclude(ur=>ur.Role)
            .Select(u => new UserViewDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Roles = u.UserRoles.Select(ur => ur.Role.RoleName).ToArray()
            })
            .ToListAsync();
        return Ok(users);
    }
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            return Conflict(new { message = "Tên đăng nhập đã tồn tại!" });

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            FullName = dto.FullName,
            Phone = dto.Phone,
            CreatedAt = DateTime.Now,
            IsActive = true
        };
        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        foreach (var rid in dto.RoleIds)
        {
            user.UserRoles.Add(new UserRole { RoleId = rid });
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "✅ Thêm user thành công!", userId = user.UserId });
    }
}
namespace BookingApi.DTOs
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int[] RoleIds { get; set; } = Array.Empty<int>();
    }

    public class LoginDto
    {
        public string UsernameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UserViewDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}

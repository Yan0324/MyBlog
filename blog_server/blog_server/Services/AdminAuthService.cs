using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using blog_server.Services.IService;
using Microsoft.IdentityModel.Tokens;

namespace blog_server.Services;

/// <summary>
/// 后台鉴权实现：BCrypt 密码哈希 + JWT Token 签发。
/// </summary>
public class AdminAuthService : IAdminAuthService
{
    private readonly string _adminUsername;
    private readonly string _passwordHash;
    private readonly string _jwtSecretKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly int _jwtExpirationMinutes;

    public AdminAuthService(IConfiguration configuration)
    {
        // 管理员用户名（默认 "admin"）
        _adminUsername = configuration["Admin:Username"] ?? "admin";

        // 密码：优先使用预计算的 BCrypt 哈希，否则对明文密码做哈希
        var passwordHash = configuration["Admin:PasswordHash"];
        if (!string.IsNullOrEmpty(passwordHash))
        {
            _passwordHash = passwordHash;
        }
        else
        {
            var password = configuration["Admin:Password"]
                ?? throw new InvalidOperationException("未配置 Admin:Password 或 Admin:PasswordHash");
            _passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        // JWT 配置
        _jwtSecretKey = configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("未配置 Jwt:SecretKey（至少 32 字符）");
        if (Encoding.UTF8.GetBytes(_jwtSecretKey).Length < 32)
        {
            throw new InvalidOperationException("Jwt:SecretKey 长度不足，至少需要 32 字符（256 位）");
        }

        _jwtIssuer = configuration["Jwt:Issuer"] ?? "MyBlog";
        _jwtAudience = configuration["Jwt:Audience"] ?? "MyBlog";
        _jwtExpirationMinutes = int.TryParse(configuration["Jwt:ExpirationMinutes"], out var minutes)
            ? minutes
            : 480; // 默认 8 小时
    }

    /// <summary>校验用户名（忽略大小写）和密码（BCrypt 恒定时间比较）。</summary>
    public bool ValidateUser(string username, string password) =>
        string.Equals(username, _adminUsername, StringComparison.OrdinalIgnoreCase)
        && BCrypt.Net.BCrypt.Verify(password, _passwordHash);

    /// <summary>签发 JWT Bearer Token，包含 Admin 角色声明和过期时间。</summary>
    public string GenerateToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

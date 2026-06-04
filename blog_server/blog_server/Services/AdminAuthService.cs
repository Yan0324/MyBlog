namespace blog_server.Services;

/// <summary>
/// 后台简单鉴权：固定密码 + 固定 Token，后续可改为 JWT 或数据库用户。
/// </summary>
public class AdminAuthService
{
    private readonly string _password;
    private readonly string _token;

    public AdminAuthService(IConfiguration configuration)
    {
        _password = configuration["Admin:Password"] ?? "blog-admin";
        _token = configuration["Admin:Token"] ?? "dev-admin-token-change-me";
    }

    /// <summary>校验登录密码。</summary>
    public bool ValidatePassword(string password) =>
        string.Equals(password, _password, StringComparison.Ordinal);

    /// <summary>返回 Bearer Token。</summary>
    public string GetToken() => _token;

    /// <summary>校验 Authorization 头中的 Bearer Token。</summary>
    public bool ValidateToken(string? authorizationHeader)
    {
        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            return false;
        }

        const string prefix = "Bearer ";
        if (!authorizationHeader.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var token = authorizationHeader[prefix.Length..].Trim();
        return string.Equals(token, _token, StringComparison.Ordinal);
    }
}

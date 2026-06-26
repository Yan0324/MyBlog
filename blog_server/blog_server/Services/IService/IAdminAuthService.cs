namespace blog_server.Services.IService;

/// <summary>
/// 后台鉴权接口（JWT + BCrypt）。
/// </summary>
public interface IAdminAuthService
{
    /// <summary>校验登录密码（BCrypt 哈希比对）。</summary>
    bool ValidatePassword(string password);

    /// <summary>签发 JWT Bearer Token。</summary>
    string GenerateToken();
}

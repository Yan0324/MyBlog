namespace blog_server.Services.IService;

/// <summary>
/// 后台鉴权接口（JWT + BCrypt）。
/// </summary>
public interface IAdminAuthService
{
    /// <summary>校验用户名和密码。</summary>
    bool ValidateUser(string username, string password);

    /// <summary>签发 JWT Bearer Token。</summary>
    string GenerateToken();
}

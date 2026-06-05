namespace blog_server.Services.IService;

/// <summary>
/// 后台鉴权接口。
/// </summary>
public interface IAdminAuthService
{
    /// <summary>校验登录密码。</summary>
    bool ValidatePassword(string password);

    /// <summary>返回 Bearer Token。</summary>
    string GetToken();

    /// <summary>校验 Authorization 头中的 Bearer Token。</summary>
    bool ValidateToken(string? authorizationHeader);
}

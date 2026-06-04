namespace blog_server.Models;

/// <summary>
/// 后台登录请求体。
/// </summary>
public class LoginRequest
{
    public string Password { get; set; } = string.Empty;
}

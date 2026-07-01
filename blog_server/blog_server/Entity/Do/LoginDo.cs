namespace blog_server.Entity.Do;

/// <summary>
/// 后台登录请求数据传输对象。
/// </summary>
public class LoginDo
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

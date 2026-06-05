namespace blog_server.Entity.Do;

/// <summary>
/// 首页状态更新请求数据传输对象。
/// </summary>
public class SiteStatusDo
{
    public string Keyword { get; set; } = string.Empty;
    public string StatusLine { get; set; } = string.Empty;
}

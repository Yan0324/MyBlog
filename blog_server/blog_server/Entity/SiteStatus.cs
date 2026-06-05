namespace blog_server.Entity;

/// <summary>
/// 首页状态数据库实体，对应 site_status 表。
/// </summary>
public class SiteStatus
{
    /// <summary>主键（单行配置，固定为 1）。</summary>
    public int Id { get; set; }

    public string Keyword { get; set; } = string.Empty;
    public string StatusLine { get; set; } = string.Empty;
}

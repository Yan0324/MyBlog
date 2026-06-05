using blog_server.Entity;

namespace blog_server.Entity.Vo;

/// <summary>
/// 首页状态返回视图对象。
/// </summary>
public class SiteStatusVo
{
    public string Keyword { get; set; } = string.Empty;
    public string StatusLine { get; set; } = string.Empty;

    public static SiteStatusVo FromEntity(SiteStatus entity) =>
        new()
        {
            Keyword = entity.Keyword,
            StatusLine = entity.StatusLine
        };
}

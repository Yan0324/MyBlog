namespace blog_server.Entity.Do;

/// <summary>
/// 文章请求数据传输对象（新建/更新）。
/// </summary>
public class ArticleDo
{
    public string Category { get; set; } = string.Empty;
    public string Kicker { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Copy { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public bool Published { get; set; } = true;
}

namespace blog_server.Entity;

/// <summary>
/// 文章数据库实体，对应 articles 表。
/// </summary>
public class Article
{
    public string Id { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Kicker { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Copy { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public bool Published { get; set; } = true;
}

namespace blog_server.Models;

/// <summary>
/// 后台新建/更新文章时的请求体。
/// </summary>
public class ArticlePayload
{
    public string Category { get; set; } = string.Empty;
    public string Kicker { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Copy { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public bool Published { get; set; } = true;
}

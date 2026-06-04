namespace blog_server.Models;

/// <summary>
/// 文章实体，字段与前端 Essay 页、后台表单一致。
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

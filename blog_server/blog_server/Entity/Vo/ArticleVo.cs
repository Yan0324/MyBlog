using blog_server.Entity;

namespace blog_server.Entity.Vo;

/// <summary>
/// 文章返回视图对象。
/// </summary>
public class ArticleVo
{
    public string Id { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Kicker { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Copy { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public bool Published { get; set; }

    public static ArticleVo FromEntity(Article entity) =>
        new()
        {
            Id = entity.Id,
            Category = entity.Category,
            Kicker = entity.Kicker,
            Title = entity.Title,
            Copy = entity.Copy,
            Content = entity.Content,
            Tags = [.. entity.Tags],
            Published = entity.Published
        };

    public static List<ArticleVo> FromEntities(IEnumerable<Article> entities) =>
        entities.Select(FromEntity).ToList();
}

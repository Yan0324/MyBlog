namespace blog_server.Entity.Vo;

/// <summary>
/// 文章列表返回包装。
/// </summary>
public class ArticleListVo
{
    public List<ArticleVo> Articles { get; set; } = [];
}

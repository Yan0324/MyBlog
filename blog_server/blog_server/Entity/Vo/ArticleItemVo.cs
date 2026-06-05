namespace blog_server.Entity.Vo;

/// <summary>
/// 单篇文章返回包装。
/// </summary>
public class ArticleItemVo
{
    public ArticleVo Article { get; set; } = new();
}

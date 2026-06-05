using blog_server.Entity.Do;
using blog_server.Entity.Vo;

namespace blog_server.Services.IService;

/// <summary>
/// 文章业务服务接口。
/// </summary>
public interface IArticleStore
{
    IReadOnlyList<ArticleVo> GetPublished(string? category);
    IReadOnlyList<ArticleVo> GetAll();
    ArticleVo? GetPublishedById(string id);
    ArticleVo Create(ArticleDo articleDo);
    ArticleVo? Update(string id, ArticleDo articleDo);
    bool Delete(string id);
}

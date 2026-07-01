using blog_server.Entity;

namespace blog_server.Mappers.IMapper;

/// <summary>
/// 文章表数据访问接口，封装 Dapper 数据库操作。
/// Dapper 每个写操作立即执行，无需 SaveChanges。
/// </summary>
public interface IArticleMapper
{
    IReadOnlyList<Article> SelectPublished(string? category);
    IReadOnlyList<Article> SelectAll();
    Article? SelectById(string id);
    void Insert(Article article);
    void Update(Article article);
    void Delete(string id);
}

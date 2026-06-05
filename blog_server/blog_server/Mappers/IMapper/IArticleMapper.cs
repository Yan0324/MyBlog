using blog_server.Entity;

namespace blog_server.Mappers.IMapper;

/// <summary>
/// 文章表数据访问接口，仅负责数据库读写。
/// </summary>
public interface IArticleMapper
{
    IReadOnlyList<Article> SelectPublished(string? category);
    IReadOnlyList<Article> SelectAll();
    Article? SelectById(string id);
    Article? SelectTrackedById(string id);
    void Insert(Article article);
    void Remove(Article article);
    void SaveChanges();
}

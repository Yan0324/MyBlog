using blog_server.Entity;
using blog_server.Entity.Do;
using blog_server.Entity.Vo;
using blog_server.Mappers.IMapper;
using blog_server.Services.IService;

namespace blog_server.Services;

/// <summary>
/// 文章业务服务：Entity ↔ Do/Vo 转换，不直接访问数据库。
/// </summary>
public class ArticleStore(IArticleMapper articleMapper) : IArticleStore
{
    public IReadOnlyList<ArticleVo> GetPublished(string? category)
    {
        var normalizedCategory = string.IsNullOrWhiteSpace(category) ? null : category.Trim();
        var entities = articleMapper.SelectPublished(normalizedCategory);
        return ArticleVo.FromEntities(entities);
    }

    public IReadOnlyList<ArticleVo> GetAll()
    {
        return ArticleVo.FromEntities(articleMapper.SelectAll());
    }

    public ArticleVo? GetPublishedById(string id)
    {
        var entity = articleMapper.SelectById(id);
        if (entity is null || !entity.Published)
        {
            return null;
        }

        return ArticleVo.FromEntity(entity);
    }

    public ArticleVo Create(ArticleDo articleDo)
    {
        var entity = MapDoToEntity(articleDo, GenerateId());
        articleMapper.Insert(entity); // Dapper: 立即执行 INSERT，无需 SaveChanges
        return ArticleVo.FromEntity(entity);
    }

    public ArticleVo? Update(string id, ArticleDo articleDo)
    {
        // Dapper 没有变更追踪：先查出实体确认存在，再执行显式 UPDATE
        var existing = articleMapper.SelectById(id);
        if (existing is null)
        {
            return null;
        }

        ApplyDo(existing, articleDo);
        articleMapper.Update(existing); // 立即执行 UPDATE
        return ArticleVo.FromEntity(existing);
    }

    public bool Delete(string id)
    {
        // Dapper: 先确认存在再删除，避免无效操作
        var existing = articleMapper.SelectById(id);
        if (existing is null)
        {
            return false;
        }

        articleMapper.Delete(id); // 直接执行 DELETE，无需 Remove + SaveChanges
        return true;
    }

    private static string GenerateId() => $"art-{Guid.NewGuid():N}"[..12];

    private static Article MapDoToEntity(ArticleDo articleDo, string id)
    {
        var entity = new Article { Id = id };
        ApplyDo(entity, articleDo);
        return entity;
    }

    private static void ApplyDo(Article entity, ArticleDo articleDo)
    {
        entity.Category = articleDo.Category.Trim();
        entity.Kicker = articleDo.Kicker.Trim();
        entity.Title = articleDo.Title.Trim();
        entity.Copy = articleDo.Copy.Trim();
        entity.Content = articleDo.Content.Trim();
        entity.Tags = articleDo.Tags?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList() ?? [];
        entity.Published = articleDo.Published;
    }
}

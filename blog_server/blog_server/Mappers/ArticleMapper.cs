using blog_server.Data;
using blog_server.Entity;
using blog_server.Mappers.IMapper;
using Microsoft.EntityFrameworkCore;

namespace blog_server.Mappers;

/// <summary>
/// 文章表 Mapper 实现，封装 EF Core 数据库操作。
/// </summary>
public class ArticleMapper(BlogDbContext db) : IArticleMapper
{
    public IReadOnlyList<Article> SelectPublished(string? category)
    {
        var query = db.Articles.AsNoTracking().Where(a => a.Published);

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(a => a.Category.ToLower() == category.ToLower());
        }

        return query.ToList();
    }

    public IReadOnlyList<Article> SelectAll()
    {
        return db.Articles.AsNoTracking().ToList();
    }

    public Article? SelectById(string id)
    {
        return db.Articles.AsNoTracking().FirstOrDefault(a => a.Id == id);
    }

    public Article? SelectTrackedById(string id)
    {
        return db.Articles.FirstOrDefault(a => a.Id == id);
    }

    public void Insert(Article article)
    {
        db.Articles.Add(article);
    }

    public void Remove(Article article)
    {
        db.Articles.Remove(article);
    }

    public void SaveChanges()
    {
        db.SaveChanges();
    }
}

using System.Data;
using blog_server.Entity;
using blog_server.Mappers.IMapper;
using Dapper;

namespace blog_server.Mappers;

/// <summary>
/// 文章表 Mapper 实现，使用 Dapper 执行 SQL。
/// 注入 IDbConnection（Scoped，每次 HTTP 请求一个连接）。
/// </summary>
public class ArticleMapper(IDbConnection db) : IArticleMapper
{
    public IReadOnlyList<Article> SelectPublished(string? category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return db.Query<Article>(
                    "SELECT * FROM articles WHERE Published = 1 ORDER BY Id DESC")
                .ToList();
        }

        return db.Query<Article>(
                "SELECT * FROM articles WHERE Published = 1 AND LOWER(Category) = LOWER(@category) ORDER BY Id DESC",
                new { category })
            .ToList();
    }

    public IReadOnlyList<Article> SelectAll()
    {
        return db.Query<Article>(
                "SELECT * FROM articles ORDER BY Id DESC")
            .ToList();
    }

    public Article? SelectById(string id)
    {
        return db.QueryFirstOrDefault<Article>(
            "SELECT * FROM articles WHERE Id = @id",
            new { id });
    }

    public void Insert(Article article)
    {
        db.Execute(@"
            INSERT INTO articles (Id, Category, Kicker, Title, Copy, Content, Tags, Published)
            VALUES (@Id, @Category, @Kicker, @Title, @Copy, @Content, @Tags, @Published)",
            article);
    }

    public void Update(Article article)
    {
        db.Execute(@"
            UPDATE articles
            SET Category = @Category,
                Kicker = @Kicker,
                Title = @Title,
                Copy = @Copy,
                Content = @Content,
                Tags = @Tags,
                Published = @Published
            WHERE Id = @Id",
            article);
    }

    public void Delete(string id)
    {
        db.Execute(
            "DELETE FROM articles WHERE Id = @id",
            new { id });
    }
}

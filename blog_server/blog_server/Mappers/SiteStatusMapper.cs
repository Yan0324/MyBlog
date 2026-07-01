using System.Data;
using blog_server.Entity;
using blog_server.Mappers.IMapper;
using Dapper;

namespace blog_server.Mappers;

/// <summary>
/// 首页状态表 Mapper 实现，使用 Dapper 执行 SQL。
/// 注入 IDbConnection（Scoped，每次 HTTP 请求一个连接）。
/// </summary>
public class SiteStatusMapper(IDbConnection db) : ISiteStatusMapper
{
    public SiteStatus? SelectById(int id)
    {
        return db.QueryFirstOrDefault<SiteStatus>(
            "SELECT * FROM site_status WHERE Id = @id",
            new { id });
    }

    public void Insert(SiteStatus status)
    {
        db.Execute(@"
            INSERT INTO site_status (Id, Keyword, StatusLine)
            VALUES (@Id, @Keyword, @StatusLine)",
            status);
    }

    public void Update(SiteStatus status)
    {
        db.Execute(@"
            UPDATE site_status
            SET Keyword = @Keyword,
                StatusLine = @StatusLine
            WHERE Id = @Id",
            status);
    }
}

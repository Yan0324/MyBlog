using blog_server.Entity;

namespace blog_server.Mappers.IMapper;

/// <summary>
/// 首页状态表数据访问接口，封装 Dapper 数据库操作。
/// Dapper 每个写操作立即执行，无需 SaveChanges。
/// </summary>
public interface ISiteStatusMapper
{
    SiteStatus? SelectById(int id);
    void Insert(SiteStatus status);
    void Update(SiteStatus status);
}

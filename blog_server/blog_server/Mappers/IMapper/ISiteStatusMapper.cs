using blog_server.Entity;

namespace blog_server.Mappers.IMapper;

/// <summary>
/// 首页状态表数据访问接口，仅负责数据库读写。
/// </summary>
public interface ISiteStatusMapper
{
    SiteStatus? SelectById(int id);
    SiteStatus? SelectTrackedById(int id);
    void Insert(SiteStatus status);
    void SaveChanges();
}

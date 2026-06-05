using blog_server.Data;
using blog_server.Entity;
using blog_server.Mappers.IMapper;
using Microsoft.EntityFrameworkCore;

namespace blog_server.Mappers;

/// <summary>
/// 首页状态表 Mapper 实现，封装 EF Core 数据库操作。
/// </summary>
public class SiteStatusMapper(BlogDbContext db) : ISiteStatusMapper
{
    public SiteStatus? SelectById(int id)
    {
        return db.SiteStatuses.AsNoTracking().FirstOrDefault(s => s.Id == id);
    }

    public SiteStatus? SelectTrackedById(int id)
    {
        return db.SiteStatuses.FirstOrDefault(s => s.Id == id);
    }

    public void Insert(SiteStatus status)
    {
        db.SiteStatuses.Add(status);
    }

    public void SaveChanges()
    {
        db.SaveChanges();
    }
}

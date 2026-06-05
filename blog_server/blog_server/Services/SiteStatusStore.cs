using blog_server.Entity;
using blog_server.Entity.Do;
using blog_server.Entity.Vo;
using blog_server.Mappers.IMapper;
using blog_server.Services.IService;

namespace blog_server.Services;

/// <summary>
/// 首页状态业务服务：Entity ↔ Do/Vo 转换，不直接访问数据库。
/// </summary>
public class SiteStatusStore(ISiteStatusMapper statusMapper) : ISiteStatusStore
{
    private const int StatusRowId = 1;
    private const string DefaultKeyword = "Be Rich";
    private const string DefaultStatusLine = "2026 · 平静";

    public SiteStatusVo Get()
    {
        var entity = statusMapper.SelectById(StatusRowId);
        if (entity is not null)
        {
            return SiteStatusVo.FromEntity(entity);
        }

        return new SiteStatusVo
        {
            Keyword = DefaultKeyword,
            StatusLine = DefaultStatusLine
        };
    }

    public SiteStatusVo Update(SiteStatusDo statusDo)
    {
        var keyword = statusDo.Keyword.Trim();
        var statusLine = statusDo.StatusLine.Trim();

        var existing = statusMapper.SelectTrackedById(StatusRowId);
        if (existing is null)
        {
            existing = new SiteStatus
            {
                Id = StatusRowId,
                Keyword = keyword,
                StatusLine = statusLine
            };
            statusMapper.Insert(existing);
        }
        else
        {
            existing.Keyword = keyword;
            existing.StatusLine = statusLine;
        }

        statusMapper.SaveChanges();
        return SiteStatusVo.FromEntity(existing);
    }
}

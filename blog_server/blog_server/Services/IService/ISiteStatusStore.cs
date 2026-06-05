using blog_server.Entity.Do;
using blog_server.Entity.Vo;

namespace blog_server.Services.IService;

/// <summary>
/// 首页状态业务服务接口。
/// </summary>
public interface ISiteStatusStore
{
    SiteStatusVo Get();
    SiteStatusVo Update(SiteStatusDo statusDo);
}

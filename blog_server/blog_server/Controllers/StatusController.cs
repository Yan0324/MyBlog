using blog_server.Common;
using blog_server.Entity.Vo;
using blog_server.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace blog_server.Controllers;

/// <summary>
/// 前台首页状态接口。
/// </summary>
[ApiController]
[Route("api/status")]
public class StatusController(ISiteStatusStore store) : ControllerBase
{
    /// <summary>
    /// 获取首页展示的状态信息。
    /// </summary>
    [HttpGet]
    public ActionResult<Result<SiteStatusItemVo>> Get()
    {
        var status = store.Get();
        return Ok(Result<SiteStatusItemVo>.Ok(new SiteStatusItemVo { Status = status }));
    }
}

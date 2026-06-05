using blog_server.Common;
using blog_server.Entity.Do;
using blog_server.Entity.Vo;
using blog_server.Filters;
using blog_server.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace blog_server.Controllers;

/// <summary>
/// 后台管理接口。
/// </summary>
[ApiController]
[Route("api/admin")]
public class AdminController(
    IArticleStore store,
    ISiteStatusStore statusStore,
    IAdminAuthService authService) : ControllerBase
{
    /// <summary>
    /// 后台登录，成功返回 Bearer Token。
    /// </summary>
    [HttpPost("login")]
    public ActionResult<Result<LoginVo>> Login([FromBody] LoginDo loginDo)
    {
        if (string.IsNullOrWhiteSpace(loginDo.Password))
        {
            return Unauthorized(Result<LoginVo>.Fail(401, "密码错误"));
        }

        if (!authService.ValidatePassword(loginDo.Password))
        {
            return Unauthorized(Result<LoginVo>.Fail(401, "密码错误"));
        }

        return Ok(Result<LoginVo>.Ok(new LoginVo { Token = authService.GetToken() }));
    }

    /// <summary>
    /// 获取首页状态配置。
    /// </summary>
    [HttpGet("status")]
    [AdminAuthorize]
    public ActionResult<Result<SiteStatusItemVo>> GetStatus()
    {
        var status = statusStore.Get();
        return Ok(Result<SiteStatusItemVo>.Ok(new SiteStatusItemVo { Status = status }));
    }

    /// <summary>
    /// 更新首页状态配置。
    /// </summary>
    [HttpPut("status")]
    [AdminAuthorize]
    public ActionResult<Result<SiteStatusItemVo>> UpdateStatus([FromBody] SiteStatusDo statusDo)
    {
        if (string.IsNullOrWhiteSpace(statusDo.Keyword))
        {
            return BadRequest(Result<SiteStatusItemVo>.Fail(400, "年度关键词不能为空"));
        }

        var status = statusStore.Update(statusDo);
        return Ok(Result<SiteStatusItemVo>.Ok(new SiteStatusItemVo { Status = status }));
    }

    /// <summary>
    /// 获取全部文章（含草稿）。
    /// </summary>
    [HttpGet("articles")]
    [AdminAuthorize]
    public ActionResult<Result<ArticleListVo>> GetAll()
    {
        var articles = store.GetAll();
        return Ok(Result<ArticleListVo>.Ok(new ArticleListVo { Articles = [.. articles] }));
    }

    /// <summary>
    /// 新建文章。
    /// </summary>
    [HttpPost("articles")]
    [AdminAuthorize]
    public ActionResult<Result<ArticleItemVo>> Create([FromBody] ArticleDo articleDo)
    {
        if (string.IsNullOrWhiteSpace(articleDo.Title))
        {
            return BadRequest(Result<ArticleItemVo>.Fail(400, "标题不能为空"));
        }

        var article = store.Create(articleDo);
        return Ok(Result<ArticleItemVo>.Ok(new ArticleItemVo { Article = article }));
    }

    /// <summary>
    /// 更新文章。
    /// </summary>
    [HttpPut("articles/{id}")]
    [AdminAuthorize]
    public ActionResult<Result<ArticleItemVo>> Update(string id, [FromBody] ArticleDo articleDo)
    {
        if (string.IsNullOrWhiteSpace(articleDo.Title))
        {
            return BadRequest(Result<ArticleItemVo>.Fail(400, "标题不能为空"));
        }

        var article = store.Update(id, articleDo);
        if (article is null)
        {
            return NotFound(Result<ArticleItemVo>.Fail(404, "文章不存在"));
        }

        return Ok(Result<ArticleItemVo>.Ok(new ArticleItemVo { Article = article }));
    }

    /// <summary>
    /// 删除文章。
    /// </summary>
    [HttpDelete("articles/{id}")]
    [AdminAuthorize]
    public ActionResult<Result> Delete(string id)
    {
        if (!store.Delete(id))
        {
            return NotFound(Result.Fail(404, "文章不存在"));
        }

        return Ok(Result.Ok(message: "已删除"));
    }
}

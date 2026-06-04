using blog_server.Filters;
using blog_server.Models;
using blog_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace blog_server.Controllers;

/// <summary>
/// 后台文章管理接口。
/// </summary>
[ApiController]
[Route("api/admin")]
public class AdminController(ArticleStore store, AdminAuthService authService) : ControllerBase
{
    /// <summary>
    /// 后台登录，成功返回 Bearer Token。
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return Unauthorized(new { message = "密码错误" });
        }

        if (!authService.ValidatePassword(request.Password))
        {
            return Unauthorized(new { message = "密码错误" });
        }

        return Ok(new { token = authService.GetToken() });
    }

    /// <summary>
    /// 获取全部文章（含草稿）。
    /// </summary>
    [HttpGet("articles")]
    [AdminAuthorize]
    public ActionResult<object> GetAll()
    {
        var articles = store.GetAll();
        return Ok(new { articles });
    }

    /// <summary>
    /// 新建文章。
    /// </summary>
    [HttpPost("articles")]
    [AdminAuthorize]
    public ActionResult<object> Create([FromBody] ArticlePayload payload)
    {
        if (string.IsNullOrWhiteSpace(payload.Title))
        {
            return BadRequest(new { message = "标题不能为空" });
        }

        var article = store.Create(payload);
        return Ok(new { article });
    }

    /// <summary>
    /// 更新文章。
    /// </summary>
    [HttpPut("articles/{id}")]
    [AdminAuthorize]
    public ActionResult<object> Update(string id, [FromBody] ArticlePayload payload)
    {
        if (string.IsNullOrWhiteSpace(payload.Title))
        {
            return BadRequest(new { message = "标题不能为空" });
        }

        var article = store.Update(id, payload);
        if (article is null)
        {
            return NotFound(new { message = "文章不存在" });
        }

        return Ok(new { article });
    }

    /// <summary>
    /// 删除文章。
    /// </summary>
    [HttpDelete("articles/{id}")]
    [AdminAuthorize]
    public IActionResult Delete(string id)
    {
        if (!store.Delete(id))
        {
            return NotFound(new { message = "文章不存在" });
        }

        return Ok(new { message = "已删除" });
    }
}

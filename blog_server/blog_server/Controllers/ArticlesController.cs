using blog_server.Models;
using blog_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace blog_server.Controllers;

/// <summary>
/// 前台文章接口。
/// </summary>
[ApiController]
[Route("api/articles")]
public class ArticlesController(ArticleStore store) : ControllerBase
{
    /// <summary>
    /// 获取已发布文章列表，可选 category 参数：tech / life / notes。
    /// </summary>
    [HttpGet]
    public ActionResult<object> GetPublished([FromQuery] string? category)
    {
        var articles = store.GetPublished(category);
        return Ok(new { articles });
    }
}

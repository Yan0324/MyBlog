using blog_server.Common;
using blog_server.Entity.Do;
using blog_server.Entity.Vo;
using blog_server.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace blog_server.Controllers;

/// <summary>
/// 前台文章接口。
/// </summary>
[ApiController]
[Route("api/articles")]
public class ArticlesController(IArticleStore store) : ControllerBase
{
    /// <summary>
    /// 获取已发布文章列表，可选 category 参数：tech / life / notes。
    /// </summary>
    [HttpGet]
    public ActionResult<Result<ArticleListVo>> GetPublished([FromQuery] string? category)
    {
        var articles = store.GetPublished(category);
        return Ok(Result<ArticleListVo>.Ok(new ArticleListVo { Articles = [.. articles] }));
    }

    /// <summary>
    /// 获取单篇已发布文章（详情页）。
    /// </summary>
    [HttpGet("{id}")]
    public ActionResult<Result<ArticleItemVo>> GetById(string id)
    {
        var article = store.GetPublishedById(id);
        if (article is null)
        {
            return NotFound(Result<ArticleItemVo>.Fail(404, "文章不存在"));
        }

        return Ok(Result<ArticleItemVo>.Ok(new ArticleItemVo { Article = article }));
    }
}

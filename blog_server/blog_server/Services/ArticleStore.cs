using blog_server.Models;

namespace blog_server.Services;

/// <summary>
/// 内存文章存储（虚拟数据），后续可替换为数据库实现。
/// </summary>
public class ArticleStore
{
    private readonly List<Article> _articles;
    private readonly Lock _lock = new();

    public ArticleStore()
    {
        // 预置示例数据，便于前后端联调
        _articles =
        [
            new Article
            {
                Id = "art-001",
                Category = "tech",
                Kicker = "2026 · 06",
                Title = "Vue 3 与 ASP.NET Core 前后端分离",
                Copy = "记录个人博客从静态页迁移到前后端分离架构的过程与踩坑。",
                Content = """
                    ## 为什么选前后端分离

                    个人博客从静态页迁移到 **Vue + ASP.NET Core** 后，内容可以走 API 管理，前台只负责展示。

                    ### 技术栈

                    - 前端：Vue 3 + Vue Router
                    - 后端：ASP.NET Core Web API
                    - 正文：支持 **Markdown** 编写

                    ```js
                    // 示例：前台拉取文章
                    const data = await fetch('/api/articles')
                    ```

                    > 列表页显示摘要，详情页渲染 Markdown 正文。
                    """,
                Tags = ["Vue", "ASP.NET", "随笔"],
                Published = true
            },
            new Article
            {
                Id = "art-002",
                Category = "life",
                Kicker = "2026 · 05",
                Title = "六月的一些小确幸",
                Copy = "天气渐暖，整理房间、读书、写代码，平凡日子里的碎片。",
                Content = "",
                Tags = ["日常", "生活"],
                Published = true
            },
            new Article
            {
                Id = "art-003",
                Category = "notes",
                Kicker = "2026 · 04",
                Title = "接口设计草稿（未发布）",
                Copy = "这是一篇草稿，前台不应看到。",
                Content = "仅供后台测试。",
                Tags = ["草稿"],
                Published = false
            }
        ];
    }

    /// <summary>获取已发布文章，可按分类筛选。</summary>
    public IReadOnlyList<Article> GetPublished(string? category)
    {
        lock (_lock)
        {
            var query = _articles.Where(a => a.Published);

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(a =>
                    string.Equals(a.Category, category, StringComparison.OrdinalIgnoreCase));
            }

            return query.ToList();
        }
    }

    /// <summary>获取全部文章（含草稿）。</summary>
    public IReadOnlyList<Article> GetAll()
    {
        lock (_lock)
        {
            return _articles.ToList();
        }
    }

    /// <summary>按 ID 查找文章。</summary>
    public Article? FindById(string id)
    {
        lock (_lock)
        {
            return _articles.FirstOrDefault(a => a.Id == id);
        }
    }

    /// <summary>按 ID 获取已发布文章（前台详情页）。</summary>
    public Article? GetPublishedById(string id)
    {
        lock (_lock)
        {
            var article = _articles.FirstOrDefault(a => a.Id == id);
            if (article is null || !article.Published)
            {
                return null;
            }

            return Clone(article);
        }
    }

    /// <summary>新建文章并返回副本。</summary>
    public Article Create(ArticlePayload payload)
    {
        lock (_lock)
        {
            var article = MapPayloadToArticle(payload, GenerateId());
            _articles.Add(article);
            return Clone(article);
        }
    }

    /// <summary>更新文章，不存在则返回 null。</summary>
    public Article? Update(string id, ArticlePayload payload)
    {
        lock (_lock)
        {
            var index = _articles.FindIndex(a => a.Id == id);
            if (index < 0)
            {
                return null;
            }

            var updated = MapPayloadToArticle(payload, id);
            _articles[index] = updated;
            return Clone(updated);
        }
    }

    /// <summary>删除文章，不存在则返回 false。</summary>
    public bool Delete(string id)
    {
        lock (_lock)
        {
            var index = _articles.FindIndex(a => a.Id == id);
            if (index < 0)
            {
                return false;
            }

            _articles.RemoveAt(index);
            return true;
        }
    }

    private static string GenerateId() => $"art-{Guid.NewGuid():N}"[..12];

    private static Article MapPayloadToArticle(ArticlePayload payload, string id) =>
        new()
        {
            Id = id,
            Category = payload.Category.Trim(),
            Kicker = payload.Kicker.Trim(),
            Title = payload.Title.Trim(),
            Copy = payload.Copy.Trim(),
            Content = payload.Content.Trim(),
            Tags = payload.Tags?.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList() ?? [],
            Published = payload.Published
        };

    private static Article Clone(Article source) =>
        new()
        {
            Id = source.Id,
            Category = source.Category,
            Kicker = source.Kicker,
            Title = source.Title,
            Copy = source.Copy,
            Content = source.Content,
            Tags = [.. source.Tags],
            Published = source.Published
        };
}

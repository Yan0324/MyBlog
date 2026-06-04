# Blog API 接口文档

个人博客后端（`blog_server`）REST API 说明。当前使用**内存虚拟数据**，后续可无缝替换为数据库持久化。

---

## 基础信息

| 项 | 说明 |
|---|---|
| 基础路径 | `/api` |
| 本地开发地址 | `http://localhost:5115` |
| 数据格式 | `application/json` |
| 字段命名 | camelCase（如 `id`、`published`） |
| 启动命令 | `dotnet run --project blog_server --launch-profile http` |

### 前端联调

开发环境下，Vue devServer（`http://localhost:8080`）通过 `vue.config.js` 将 `/api` 代理到 `http://localhost:5115`。

生产环境可在 `web_page/.env.production` 中配置 `VUE_APP_API_BASE`。

### 通用错误格式

请求失败时，响应体通常为：

```json
{
  "message": "错误描述"
}
```

| HTTP 状态码 | 含义 |
|---|---|
| 200 | 成功 |
| 400 | 请求参数不合法 |
| 401 | 未授权（密码错误或 Token 无效） |
| 404 | 资源不存在 |

---

## 数据模型

### Article（文章）

| 字段 | 类型 | 必填 | 说明 |
|---|---|---|---|
| `id` | string | 是（响应） | 文章唯一 ID，服务端生成，格式如 `art-001` |
| `category` | string | 是 | 分类：`tech` / `life` / `notes` |
| `kicker` | string | 否 | 角标或日期，如 `"2026 · 06"` |
| `title` | string | 是 | 标题 |
| `copy` | string | 否 | 列表页摘要 |
| `content` | string | 否 | 正文（预留详情页） |
| `tags` | string[] | 否 | 标签数组 |
| `published` | boolean | 否 | 是否发布，默认 `true`；`false` 为草稿，前台不可见 |

### ArticlePayload（新建/更新请求体）

与 `Article` 相同，但**不含** `id` 字段（更新时 `id` 放在 URL 路径中）。

---

## 前台接口

### 获取已发布文章列表

```
GET /api/articles
GET /api/articles?category={category}
```

**认证：** 无

**Query 参数：**

| 参数 | 类型 | 必填 | 说明 |
|---|---|---|---|
| `category` | string | 否 | 按分类筛选：`tech` / `life` / `notes`；省略则返回全部已发布文章 |

**过滤规则：** 仅返回 `published !== false` 的文章。

**响应示例（200）：**

```json
{
  "articles": [
    {
      "id": "art-001",
      "category": "tech",
      "kicker": "2026 · 06",
      "title": "Vue 3 与 ASP.NET Core 前后端分离",
      "copy": "记录个人博客从静态页迁移到前后端分离架构的过程与踩坑。",
      "content": "正文占位：后续接入详情页时再展示完整内容。",
      "tags": ["Vue", "ASP.NET", "随笔"],
      "published": true
    }
  ]
}
```

**curl 示例：**

```bash
curl http://localhost:5115/api/articles
curl "http://localhost:5115/api/articles?category=tech"
```

**前端调用：** `fetchPublishedArticles(category?)` — `web_page/src/api/client.js`

---

## 后台接口

后台接口需先登录获取 Token，后续请求在 Header 中携带：

```
Authorization: Bearer <token>
```

开发环境默认 Token 见 `appsettings.Development.json` 中 `Admin:Token`。

---

### 后台登录

```
POST /api/admin/login
```

**认证：** 无

**请求体：**

```json
{
  "password": "blog-admin"
}
```

| 字段 | 类型 | 必填 | 说明 |
|---|---|---|---|
| `password` | string | 是 | 管理密码，开发环境默认 `blog-admin` |

**响应示例（200）：**

```json
{
  "token": "dev-admin-token-change-me"
}
```

**错误响应（401）：**

```json
{
  "message": "密码错误"
}
```

**curl 示例：**

```bash
curl -X POST http://localhost:5115/api/admin/login \
  -H "Content-Type: application/json" \
  -d "{\"password\":\"blog-admin\"}"
```

**前端调用：** `adminLogin(password)` — `web_page/src/api/client.js`

---

### 获取全部文章（含草稿）

```
GET /api/admin/articles
```

**认证：** Bearer Token

**响应示例（200）：**

```json
{
  "articles": [
    {
      "id": "art-003",
      "category": "notes",
      "kicker": "2026 · 04",
      "title": "接口设计草稿（未发布）",
      "copy": "这是一篇草稿，前台不应看到。",
      "content": "仅供后台测试。",
      "tags": ["草稿"],
      "published": false
    }
  ]
}
```

**错误响应（401）：**

```json
{
  "message": "未授权"
}
```

**curl 示例：**

```bash
curl http://localhost:5115/api/admin/articles \
  -H "Authorization: Bearer dev-admin-token-change-me"
```

**前端调用：** `fetchAdminArticles(token)` — `web_page/src/api/client.js`

---

### 新建文章

```
POST /api/admin/articles
```

**认证：** Bearer Token

**请求体：**

```json
{
  "category": "notes",
  "kicker": "2026 · 06",
  "title": "文章标题",
  "copy": "摘要",
  "content": "正文",
  "tags": ["Vue", "随笔"],
  "published": true
}
```

| 字段 | 类型 | 必填 | 说明 |
|---|---|---|---|
| `category` | string | 是 | `tech` / `life` / `notes` |
| `kicker` | string | 否 | 角标 / 日期 |
| `title` | string | 是 | 标题，不能为空 |
| `copy` | string | 否 | 摘要 |
| `content` | string | 否 | 正文 |
| `tags` | string[] | 否 | 标签 |
| `published` | boolean | 否 | 是否立即发布，默认 `true` |

**响应示例（200）：**

```json
{
  "article": {
    "id": "art-a1b2c3d4e5f6",
    "category": "notes",
    "kicker": "2026 · 06",
    "title": "文章标题",
    "copy": "摘要",
    "content": "正文",
    "tags": ["Vue", "随笔"],
    "published": true
  }
}
```

**错误响应（400）：**

```json
{
  "message": "标题不能为空"
}
```

**curl 示例：**

```bash
curl -X POST http://localhost:5115/api/admin/articles \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer dev-admin-token-change-me" \
  -d "{\"category\":\"notes\",\"kicker\":\"2026 · 06\",\"title\":\"测试文章\",\"copy\":\"摘要\",\"content\":\"正文\",\"tags\":[\"测试\"],\"published\":true}"
```

**前端调用：** `createArticle(token, payload)` — `web_page/src/api/client.js`

---

### 更新文章

```
PUT /api/admin/articles/{id}
```

**认证：** Bearer Token

**路径参数：**

| 参数 | 类型 | 说明 |
|---|---|---|
| `id` | string | 文章 ID |

**请求体：** 与「新建文章」相同

**响应示例（200）：**

```json
{
  "article": {
    "id": "art-001",
    "category": "tech",
    "kicker": "2026 · 06",
    "title": "更新后的标题",
    "copy": "更新后的摘要",
    "content": "更新后的正文",
    "tags": ["Vue"],
    "published": true
  }
}
```

**错误响应：**

| 状态码 | 示例 |
|---|---|
| 400 | `{ "message": "标题不能为空" }` |
| 401 | `{ "message": "未授权" }` |
| 404 | `{ "message": "文章不存在" }` |

**curl 示例：**

```bash
curl -X PUT http://localhost:5115/api/admin/articles/art-001 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer dev-admin-token-change-me" \
  -d "{\"category\":\"tech\",\"title\":\"更新后的标题\",\"copy\":\"摘要\",\"content\":\"正文\",\"tags\":[],\"published\":true}"
```

**前端调用：** `updateArticle(token, id, payload)` — `web_page/src/api/client.js`

---

### 删除文章

```
DELETE /api/admin/articles/{id}
```

**认证：** Bearer Token

**路径参数：**

| 参数 | 类型 | 说明 |
|---|---|---|
| `id` | string | 文章 ID |

**响应示例（200）：**

```json
{
  "message": "已删除"
}
```

**错误响应：**

| 状态码 | 示例 |
|---|---|
| 401 | `{ "message": "未授权" }` |
| 404 | `{ "message": "文章不存在" }` |

**curl 示例：**

```bash
curl -X DELETE http://localhost:5115/api/admin/articles/art-001 \
  -H "Authorization: Bearer dev-admin-token-change-me"
```

**前端调用：** `deleteArticle(token, id)` — `web_page/src/api/client.js`

---

## 接口一览

| 方法 | 路径 | 认证 | 说明 |
|---|---|---|---|
| GET | `/api/articles` | 否 | 获取已发布文章列表 |
| POST | `/api/admin/login` | 否 | 后台登录 |
| GET | `/api/admin/articles` | Bearer | 获取全部文章（含草稿） |
| POST | `/api/admin/articles` | Bearer | 新建文章 |
| PUT | `/api/admin/articles/{id}` | Bearer | 更新文章 |
| DELETE | `/api/admin/articles/{id}` | Bearer | 删除文章 |

---

## 开发配置

开发环境配置位于 `blog_server/appsettings.Development.json`：

```json
{
  "Admin": {
    "Password": "blog-admin",
    "Token": "dev-admin-token-change-me"
  }
}
```

> **注意：** 当前数据存储在内存中，服务重启后除预置示例外，新建/修改的内容会丢失。接入数据库后此 API 契约保持不变。

---

## OpenAPI

开发环境下可访问 OpenAPI 描述文件：

```
GET http://localhost:5115/openapi/v1.json
```

也可使用项目内 `blog_server.http` 文件在 IDE 中直接发送测试请求。

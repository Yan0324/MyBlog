# CLAUDE.md

本文件为 Claude Code（claude.ai/code）提供在本仓库中工作的指引。

## 项目概览

Cyan（"A better day"）—— 个人博客。前端 Vue 3 SPA，后端 ASP.NET Core Web API，数据库 MySQL 8.0。生产环境由 Nginx 托管构建后的 SPA 静态文件，并将 `/api/*` 反向代理到 .NET 后端。

## 开发命令

### 前端（`web_page/`）

```bash
cd web_page
npm run serve       # 开发服务器 localhost:8080，/api 代理到 localhost:5115
npm run build       # 生产构建，输出到 dist/
npm run lint        # ESLint（vue3-essential + recommended 规则）
```

### 后端（`blog_server/blog_server/`）

```bash
cd blog_server/blog_server
dotnet run          # 开发服务器 localhost:5115（launchSettings.json）
dotnet build        # 仅编译
```

**无 EF Core，无数据库迁移**：项目已从 EF Core 迁移到 Dapper，数据库表通过 `DbInitializer` 在启动时自动创建（`CREATE TABLE IF NOT EXISTS`），无需手动执行迁移命令。

## 架构

### 后端分层（由外到内）

```
Controller  ←  Service (Store)  ←  Mapper (Dapper)  ←  MySQL
    ↑                ↑                ↑
  HTTP 层        业务逻辑层        数据访问层
  (路由、         Entity↔Vo/Do     (手写 SQL、
   认证特性)       转换)            立即执行)
```

- **Controllers** — 薄 HTTP 层：校验输入，调用 Service，返回 `Result<T>`。后台接口通过 `[Authorize]` 进行 JWT 认证。
- **Services**（`*Store`）— 业务逻辑。Entity ↔ DTO 转换在此完成。Service 绝不直接接触 `IDbConnection`，而是通过 Mapper 访问数据。
- **Mappers** — 基于 **Dapper** 的数据访问层（原为 EF Core）。每个 Mapper 注入 `IDbConnection`（Scoped），手写 SQL，每次操作立即执行（无 `SaveChanges`），无变更追踪。
- **Entities** — 数据库行类型：`Article`、`SiteStatus`。相关的请求体 DTO 放在 `Entity/Do/`，响应体 DTO 放在 `Entity/Vo/`。
- **Common/Result.cs** — 统一 API 响应信封 `{ code: int, message: string, data: T? }`。所有响应都使用此结构。`Result.Ok()` 返回 code 200；`Result.Fail(code, msg)` 返回错误。
- **Data/DbInitializer.cs** — 应用启动时执行 `CREATE TABLE IF NOT EXISTS`，确保数据库表存在（幂等操作）。
- **Data/JsonTypeHandler.cs** — Dapper 自定义类型处理器，自动处理 `List<string>` ↔ MySQL JSON 列的序列化/反序列化。

### 前端（`web_page/src/`）

```
App.vue（加载动画 → ParticleCanvas + NavBar + router-view + footer）
  ├── views/          页面组件（HomeView, ProjectsView, ArticleDetailView, AdminView）
  ├── components/     可复用组件（NavBar, HeroSection, MarkdownContent 等）
  ├── api/client.js   所有 API 调用 —— fetch() 的薄封装
  ├── router/         Hash 模式路由（createWebHashHistory）
  └── utils/          markdown.js（marked + DOMPurify）
```

**后台页面隔离**：`App.vue` 通过 `isAdminRoute` 计算属性判断当前路由是否为 `/admin`，后台页面不渲染公用顶栏（NavBar）、粒子背景、Lottie 动画和 footer。后台使用 `v-show` 切换登录界面和 Dashboard，避免 Element Plus 组件 DOM 重建导致布局异常。

### 代码注释规范

- **必须添加中文注释**：所有关键功能、复杂逻辑、算法、业务规则处必须用中文写注释，说明"做什么"和"为什么"。
- 注释粒度：公共方法/函数、重要的条件分支、非显而易见的计算、重要的配置项都需要注释。
- 注释风格应与项目中已有注释保持一致（XML 文档注释 `<summary>` 用于 C#，`/** */` 或 `//` 用于 JavaScript/Vue）。

### 关键模式

- **统一响应信封**：后端始终返回 `{ code, message, data }`。前端 `client.js` 在成功时提取 `data` 字段，失败时抛出 `Error(message)`，因此业务代码始终直接拿到 `data`。
- **文章 ID 规则**：格式为 `art-{GUID 前 12 位}`（例如 `art-a1b2c3d4e5f6`）。在 `ArticleStore` 中通过 `$"art-{Guid.NewGuid():N}"[..12]` 生成。
- **标签存储**：以 JSON 列存入 MySQL，通过 Dapper 全局 `JsonTypeHandler<List<string>>` 自动序列化/反序列化。
- **JWT 认证流程**：后台输入用户名密码登录 → 获取 JWT token → token 存入 `localStorage`（键名 `blog_admin_token`）→ 后台 API 调用时通过 `Authorization: Bearer <token>` 传递。
- **后台认证**：
  - 用户名：在 appsettings 的 `Admin:Username` 中配置（默认 `"admin"`），校验时忽略大小写。
  - 密码：在 appsettings 的 `Admin:Password` 中配置明文，或 `Admin:PasswordHash` 中配置 BCrypt 哈希。`AdminAuthService` 优先使用哈希，否则对明文自动做 BCrypt 哈希后验证。
- **Dapper 连接管理**：`IDbConnection` 注册为 Scoped（每次 HTTP 请求一个 `MySqlConnection`），操作结束时由 DI 容器 Dispose 并归还连接池。每个 SQL 操作立即执行，无变更追踪、无 `SaveChanges`。
- **数据库初始化**：`DbInitializer.Initialize()` 在应用启动时执行 `CREATE TABLE IF NOT EXISTS`，表结构与原 EF Core Migration 一致，可重复执行。
- **开发代理**：Vue CLI 开发服务器将 `/api` 代理到 `http://localhost:5115`（`vue.config.js` 中配置）。
- **生产环境**：Nginx 将 `/api/*` 反向代理到 `http://localhost:5000`；SPA 回退 `try_files $uri /index.html`。
- **路由**：Hash 模式（`/#/`、`/#/essay`、`/#/essay/:id`、`/#/admin`）。

### 数据库

Dapper 管理两张表（`DbInitializer` 自动创建）：

- `articles` — id（主键，32 字符）、category、kicker、title、copy（text）、content（longtext）、tags（json）、published（bool）
- `site_status` — 单行数据（主键=1），存储首页 Hero 区域的 keyword 和 status_line

### 配置

- `appsettings.json` — 基础配置，数据库连接字符串和 JWT 密钥为空（部署时在 `appsettings.Production.json` 中填写）
- `appsettings.Development.json` — 本地 MySQL（端口 3306）、开发环境 JWT 密钥、后台用户名密码
- `web_page/.env.development` / `.env.production` — 设置 `VUE_APP_API_BASE=/api`
- 开发环境 CORS：允许 `localhost:8080` 和 `localhost:8081` 来源

### 部署

完整说明见 `DEPLOY.md`（782 行）。关键信息：

- 服务器：Ubuntu 22.04/24.04，需安装 .NET 10 SDK、Node.js 20、MySQL 8.0、Nginx
- 后端作为 systemd 服务运行在 5000 端口
- HTTPS 通过 Let's Encrypt certbot 获取证书
- `blog_server/Filters/` 是预留的空目录，用于将来添加 ASP.NET action/exception 过滤器

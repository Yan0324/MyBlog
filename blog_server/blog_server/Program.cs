using blog_server.Data;
using blog_server.Mappers;
using blog_server.Mappers.IMapper;
using blog_server.Services;
using blog_server.Services.IService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// MySQL 数据库连接
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("未配置 ConnectionStrings:DefaultConnection");
}

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.0-mysql")));

// 注册 Mapper（数据库访问层）
builder.Services.AddScoped<IArticleMapper, ArticleMapper>();
builder.Services.AddScoped<ISiteStatusMapper, SiteStatusMapper>();

// 注册 Service（业务逻辑层）
builder.Services.AddScoped<IArticleStore, ArticleStore>();
builder.Services.AddScoped<ISiteStatusStore, SiteStatusStore>();
builder.Services.AddSingleton<IAdminAuthService, AdminAuthService>();
builder.Services.AddScoped<blog_server.Filters.AdminAuthFilter>();

// 开发时允许 Vue devServer 跨域访问
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:8080",
                "http://127.0.0.1:8080",
                "http://localhost:8081",
                "http://127.0.0.1:8081")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("DevCors");
    // 开发环境走 HTTP 代理，避免 HTTPS 重定向导致联调失败
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

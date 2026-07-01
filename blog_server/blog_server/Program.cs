using System.Data;
using System.Text;
using blog_server.Common;
using blog_server.Data;
using blog_server.Mappers;
using blog_server.Mappers.IMapper;
using blog_server.Services;
using blog_server.Services.IService;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 注册 Dapper 自定义类型处理器：List<string> ↔ MySQL JSON 列
SqlMapper.AddTypeHandler(new JsonTypeHandler<List<string>>());

// MySQL 数据库连接字符串
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("未配置 ConnectionStrings:DefaultConnection");
}

// Dapper + MySqlConnector：注册 IDbConnection 为 Scoped（每次 HTTP 请求一个连接）
builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));

// 注册 Mapper（数据库访问层）
builder.Services.AddScoped<IArticleMapper, ArticleMapper>();
builder.Services.AddScoped<ISiteStatusMapper, SiteStatusMapper>();

// 注册 Service（业务逻辑层）
builder.Services.AddScoped<IArticleStore, ArticleStore>();
builder.Services.AddScoped<ISiteStatusStore, SiteStatusStore>();
builder.Services.AddSingleton<IAdminAuthService, AdminAuthService>();

// JWT 认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = Result.Fail(401, "未授权");
                return context.Response.WriteAsJsonAsync(result);
            }
        };
    });

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

// 数据库初始化：确保表存在（CREATE TABLE IF NOT EXISTS，幂等操作）
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    DbInitializer.Initialize((MySqlConnection)db);
}

// 生产环境在 nginx 后面，需要信任反向代理的转发头
if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor
                         | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
    });
    app.UseHttpsRedirection();
}
else
{
    app.MapOpenApi();
    app.UseCors("DevCors");
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

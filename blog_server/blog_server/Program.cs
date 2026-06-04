var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 内存文章存储与后台鉴权（后续可替换为 EF Core + 数据库）
builder.Services.AddSingleton<blog_server.Services.ArticleStore>();
builder.Services.AddSingleton<blog_server.Services.AdminAuthService>();
builder.Services.AddScoped<blog_server.Filters.AdminAuthFilter>();

// 开发时允许 Vue devServer 跨域访问
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .WithOrigins("http://localhost:8080", "http://127.0.0.1:8080")
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

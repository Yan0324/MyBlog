using System.Text.Json;
using blog_server.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace blog_server.Data;

/// <summary>
/// 博客 MySQL 数据库上下文。
/// </summary>
public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<SiteStatus> SiteStatuses => Set<SiteStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tags 以 JSON 存入 MySQL，需同时配置转换器与比较器
        var tagsConverter = new ValueConverter<List<string>, string>(
            tags => JsonSerializer.Serialize(tags, (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null) ?? new List<string>());

        var tagsComparer = new ValueComparer<List<string>>(
            (left, right) => (left ?? new List<string>()).SequenceEqual(right ?? new List<string>()),
            tags => tags.Aggregate(0, (hash, tag) => HashCode.Combine(hash, tag.GetHashCode())),
            tags => tags.ToList());

        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("articles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(32);
            entity.Property(e => e.Category).HasMaxLength(32);
            entity.Property(e => e.Kicker).HasMaxLength(64);
            entity.Property(e => e.Title).HasMaxLength(256);
            entity.Property(e => e.Copy).HasColumnType("text");
            entity.Property(e => e.Content).HasColumnType("longtext");
            entity.Property(e => e.Tags)
                .HasColumnType("json")
                .HasConversion(tagsConverter, tagsComparer);
        });

        modelBuilder.Entity<SiteStatus>(entity =>
        {
            entity.ToTable("site_status");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Keyword).HasMaxLength(128);
            entity.Property(e => e.StatusLine).HasMaxLength(128);
        });
    }
}

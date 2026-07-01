using Dapper;
using MySqlConnector;

namespace blog_server.Data;

/// <summary>
/// 数据库初始化：使用 CREATE TABLE IF NOT EXISTS 确保所需表存在。
/// 表结构来源于原 EF Core Migration，可重复执行，幂等。
/// </summary>
public static class DbInitializer
{
    public static void Initialize(MySqlConnection connection)
    {
        // 文章表
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS articles (
                Id VARCHAR(32) NOT NULL,
                Category VARCHAR(32) NOT NULL DEFAULT '',
                Kicker VARCHAR(64) NOT NULL DEFAULT '',
                Title VARCHAR(256) NOT NULL DEFAULT '',
                Copy TEXT NOT NULL,
                Content LONGTEXT NOT NULL,
                Tags JSON NOT NULL,
                Published TINYINT(1) NOT NULL DEFAULT 1,
                PRIMARY KEY (Id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
        ");

        // 首页状态表（单行配置）
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS site_status (
                Id INT NOT NULL AUTO_INCREMENT,
                Keyword VARCHAR(128) NOT NULL DEFAULT '',
                StatusLine VARCHAR(128) NOT NULL DEFAULT '',
                PRIMARY KEY (Id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
        ");
    }
}

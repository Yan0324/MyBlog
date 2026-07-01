using System.Data;
using System.Text.Json;
using Dapper;

namespace blog_server.Data;

/// <summary>
/// Dapper 自定义类型处理器：将对象序列化为 JSON 字符串存入 MySQL，
/// 读取时从 JSON 字符串反序列化回原类型。
/// </summary>
/// <typeparam name="T">需要以 JSON 格式存储的类型</typeparam>
public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
    }

    public override T? Parse(object value)
    {
        if (value is null || value is DBNull)
            return default;

        var json = value.ToString();
        if (string.IsNullOrWhiteSpace(json))
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (JsonException)
        {
            // JSON 解析失败时返回默认值，避免单条数据异常导致整个查询崩溃
            return default;
        }
    }
}

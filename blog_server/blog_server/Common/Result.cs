namespace blog_server.Common;

/// <summary>
/// 统一 API 返回结构。
/// </summary>
public class Result
{
    /// <summary>业务状态码，200 表示成功。</summary>
    public int Code { get; set; }

    /// <summary>提示信息。</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>返回数据。</summary>
    public object? Data { get; set; }

    public static Result Ok(object? data = null, string message = "success") =>
        new() { Code = 200, Message = message, Data = data };

    public static Result Fail(int code, string message) =>
        new() { Code = code, Message = message, Data = null };
}

/// <summary>
/// 带泛型数据的统一返回结构。
/// </summary>
public class Result<T>
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static Result<T> Ok(T data, string message = "success") =>
        new() { Code = 200, Message = message, Data = data };

    public static Result<T> Fail(int code, string message) =>
        new() { Code = code, Message = message, Data = default };
}

using blog_server.Common;
using blog_server.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace blog_server.Filters;

/// <summary>
/// 校验后台接口的 Bearer Token。
/// </summary>
public class AdminAuthFilter(IAdminAuthService authService) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var authorization = context.HttpContext.Request.Headers.Authorization.ToString();

        if (!authService.ValidateToken(authorization))
        {
            context.Result = new UnauthorizedObjectResult(Result.Fail(401, "未授权"));
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}

/// <summary>
/// 标记需要后台鉴权的 Action。
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminAuthorizeAttribute : ServiceFilterAttribute
{
    public AdminAuthorizeAttribute() : base(typeof(AdminAuthFilter))
    {
    }
}

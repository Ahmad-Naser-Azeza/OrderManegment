using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        //// Use ActionContext to get metadata
        var controller = context.GetRouteData()?.Values["controller"]?.ToString();
        var action = context.GetRouteData()?.Values["action"]?.ToString();

        // Skip authorization for actions in the skip list
        if (SharedKernel.SkipAuthorizationActions.ShouldSkip(controller, action))
        {
            await _next(context); // Proceed without authorization for the action
            return;
        }
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            await RespondUnauthorizedAsync(context, "Token is empty.");
            return;
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("hcGrq+iClHF7bEYWLtmR+x/rL0iyCF/funAeeBUc=");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,                
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var claims =  jwtToken.Claims;
            var role = claims.FirstOrDefault().Value;
  
            // Get the permissions for the role
            if (role != null && SharedKernel.RolePermissions.RolePermissionsMapping.TryGetValue(role, out var rolePermissions))
            {                
                //var endpoint = context.GetEndpoint();
                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                var methodInfo = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
                var authorizePermissionAttribute = methodInfo?.MethodInfo.GetCustomAttributes(typeof(SharedKernel.AuthorizePermissionAttribute), true)
                    .FirstOrDefault() as SharedKernel.AuthorizePermissionAttribute;

                if (authorizePermissionAttribute != null)
                {
                    // Check if the user has the required permission
                    if (!rolePermissions.Contains(authorizePermissionAttribute.Permission))
                    {
                        await RespondUnauthorizedAsync(context, "You do not have the necessary permission.");
                        return;
                    }
                }
            }
            else
            {
                await RespondUnauthorizedAsync(context, "Role not found or has no permissions.");
                return;
            }
        }
        catch
        {
            await RespondUnauthorizedAsync(context, "Token is invalid or expired.");
            return;
        }

        await _next(context);
    }

    private static Task RespondUnauthorizedAsync(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync($"{{\"message\":\"{message}\"}}");
    }
}


namespace ProjectPRN221_LIBManagement.authorization
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lấy URL hiện tại
            var path = context.Request.Path.Value?.ToLower();
            
            var role = context.Session.GetInt32("UserRole");


            if (path.Contains("/admin"))
            {
              
                if (!context.Session.TryGetValue("UserID", out _))
                {
                    
                    context.Response.Redirect("/Home/AccessDenied");
                    return;
                }

                
                if (role != 1)
                {
                    context.Response.Redirect("/Home/AccessDenied");
                    return;
                }
            }
            await _next(context);
        }
    }
}

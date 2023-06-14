using System.IdentityModel.Tokens.Jwt;

namespace MyBookList.Authentication.Middleware;
    
public class JwtTokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtTokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the "Authorization" header is present in the request
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            // Extract the token from the "Authorization" header
            var token = authHeader.FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtSecret = "SuperSecretKey123456789123456789"; // Replace with your JWT secret key
                    var validationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(jwtSecret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };

                    // Validate and parse the token
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                    
                    // You can access claims from the validated token like:
                    // var userId = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

                    // Set the validated principal on the HttpContext
                    context.User = principal;
                }
                catch (Exception)
                {
                    // Token validation failed
                    context.Response.StatusCode = 401;
                    return;
                }
            }
        }

        await _next(context);
    }
}

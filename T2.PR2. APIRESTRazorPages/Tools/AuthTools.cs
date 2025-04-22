using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using T2.PR2._APIRESTRazorPages.Models;
using System.Text.Json;

namespace T2.PR2._APIRESTRazorPages.Tools
{
    public class AuthTools
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthTools(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool Success, string Message)> Login(LoginUser loginModel)
        {
            loginModel.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(loginModel.Password));

            var content = new StringContent(
                JsonSerializer.Serialize(loginModel),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync("/api/Auth/login", content);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                
                return (false, body);
            }

            var token = body.Trim();

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt;
            try
            {
                jwt = handler.ReadJwtToken(token);
            }
            catch
            {
                return (false, "Invalid token format");
            }

            var identity = new ClaimsIdentity(
                jwt.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = loginModel.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                    AllowRefresh = true
                });

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
               "authToken",
               token,
               new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,
                   SameSite = SameSiteMode.Strict,
                   Expires = loginModel.RememberMe
                       ? DateTimeOffset.UtcNow.AddDays(7)
                       : DateTimeOffset.UtcNow.AddHours(1)
               });
            return (true, "");
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            var apiTokenCookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("ApiToken", apiTokenCookieOptions);
        }
    }
}

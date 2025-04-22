using System.Net.Http.Headers;

namespace T2.PR2._APIRESTRazorPages.Tools
{
    public class AuthDelTools : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthDelTools(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["authToken"];

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    
    }
}

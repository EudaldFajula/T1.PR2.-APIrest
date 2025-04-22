using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T2.PR2._APIRESTRazorPages.Models;
using T2.PR2._APIRESTRazorPages.Tools;

namespace T2.PR2._APIRESTRazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthTools _authTools;

        public LoginModel(AuthTools authTools)
        {
            _authTools = authTools;
        }

        [BindProperty]
        public LoginUser User { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            TempData.Clear();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            if (!ModelState.IsValid)
                return Page();

            var result = await _authTools.Login(User);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }

            TempData["SuccessMessage"] = "Login successful! Redirecting...";
            return Page();
        }
    }
}

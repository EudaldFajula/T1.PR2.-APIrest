using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace T2.PR2._APIRESTRazorPages.Pages
{
	public class PrivacyModel : PageModel
	{
		private readonly ILogger<PrivacyModel> _logger;

		public PrivacyModel(ILogger<PrivacyModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
		}
	}

}

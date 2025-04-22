using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T2.PR2._APIRESTRazorPages.Models;
using T2.PR2._APIRESTRazorPages.Tools;

namespace T2.PR2._APIRESTRazorPages.Pages
{
    public class GameListModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly GameTools _gameTools;

        public GameListModel(IConfiguration configuration, GameTools gameTools)
        {
            _configuration = configuration;
            _gameTools = gameTools;
        }

        public List<Game> Games { get; set; } = new();

        public string ApiErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {

            try
            {
                Games = await _gameTools.GetGamesAsync();
            }
            catch (HttpRequestException ex)
            {
                ApiErrorMessage = ex.Message;
                ModelState.AddModelError(string.Empty, ApiErrorMessage);
            }
            catch (Exception ex)
            {
                ApiErrorMessage = $"Exception calling API: {ex.Message}";
                ModelState.AddModelError(string.Empty, ApiErrorMessage);
            }

            return Page();
        }
    }
}

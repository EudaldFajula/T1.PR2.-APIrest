using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T2.PR2._APIRESTRazorPages.Models;
using T2.PR2._APIRESTRazorPages.Tools;

namespace T2.PR2._APIRESTRazorPages.Pages
{
    public class DetailGameModel : PageModel
    {
        public readonly GameTools _gameTools;

        public DetailGameModel(GameTools gameTools)
        {
            _gameTools = gameTools;
        }

        public Game Game { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Game = await _gameTools.GetGameByIdAsync(id);
            return Page();
        }
    }
}

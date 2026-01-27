using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UltimateForum.Razor.Pages.User;

namespace UltimateForum.Razor.Pages;

public class Dashboard : PageModel
{
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetLong("uid") != 2)
        {
            return RedirectToPage("/index"); 
        }
        return Page();
    }
    
}
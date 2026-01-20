using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UltimateForum.Razor.Pages;

public class Topic : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int TopicId { get; set; }
    public void OnGet()
    {
        
    }
}
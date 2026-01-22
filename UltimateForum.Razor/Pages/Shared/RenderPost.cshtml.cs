using Microsoft.AspNetCore.Mvc.RazorPages;
using UltimateForum.Razor.Db;

namespace UltimateForum.Razor.Pages.Shared;

public class RenderPost: PageModel
{
    public required ForumDbContext DbContext { get; set; }
    public new required string Content { get; set; }
    public void OnGet()
    {
        
    }
}
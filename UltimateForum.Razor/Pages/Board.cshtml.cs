using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;

namespace UltimateForum.Razor.Pages;

public class Board(ForumDbContext forumDbContext) : PageModel
{
    private readonly ForumDbContext _forumDbContext = forumDbContext;
    public Db.Models.Board BoardSpec = null!;
    [BindProperty(SupportsGet = true)]
    public long? BoardId { get; set; }
    public IActionResult OnGet()
    {
        if (!BoardId.HasValue)
        {
            return BadRequest();
        }

        if (_forumDbContext.Boards.FirstOrDefault(i => i.Id == BoardId) is null)
        {
            return RedirectToPage("/404");
        }
        BoardSpec = _forumDbContext.Boards.Include(i=>i.Topics).ThenInclude(i=>i.Posts.OrderByDescending(i=>i.CreatedAt)).ThenInclude(i=>i.Creator).FirstOrDefault(i => i.Id == BoardId) ?? throw new InvalidOperationException("This should not happen. ");
        return Page(); 
    }
}
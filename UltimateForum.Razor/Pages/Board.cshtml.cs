using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Pages.User;

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
        BoardSpec = _forumDbContext.Boards.Include(i=>i.Topics).ThenInclude(i=>i.Creater).Include(i=>i.Topics).ThenInclude(i=>i.Posts.OrderByDescending(i=>i.CreatedAt)).ThenInclude(i=>i.Creator).FirstOrDefault(i => i.Id == BoardId) ?? throw new InvalidOperationException("This should not happen. ");
        return Page(); 
    }
    public bool IsAuthorized() => _forumDbContext.Users.Find(HttpContext.Session.GetLong("uid")) != null;
    public bool IsAdmin() => HttpContext.Session.GetLong("uid") == 2;

    public IActionResult OnGetDeleteBoard(long id)
    {
        var t = _forumDbContext.Boards.Find(id);
        if (t is null)
        {
            return NotFound(); 
        }
        if (IsAdmin() ||  t.CreatedById == HttpContext.Session.GetLong("uid") )
        {
            _forumDbContext.Boards.Remove(t);
            _forumDbContext.SaveChanges();
            return RedirectToPage("/Index");
        }
        else
        {
            return Forbid(); 
        }
    }
}
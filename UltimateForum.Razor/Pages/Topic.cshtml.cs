using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Db.Models;
using UltimateForum.Razor.Pages.User;

namespace UltimateForum.Razor.Pages;

public class Topic(ForumDbContext forumDbContext, BinaryDbContext binaryDbContext) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int? TopicId { get; set; }
 
    public Db.Models.Topic? TopicData { get; set; }
    private readonly ForumDbContext _db = forumDbContext;
    /// <summary>
    /// Use with caution.
    /// </summary>
    public ForumDbContext DbContext => _db; 
    private readonly BinaryDbContext _binaryDbContext = binaryDbContext;
    public IActionResult OnGet()
    {
        if (TopicId is null)
        {
            return RedirectToPage("/404");
        }
        var s = _db.Topics.Include(i=>i.Creater).Include(i=>i.Board).Include(i=>i.Posts.OrderBy(i=>i.CreatedAt)).ThenInclude(i=>i.Creator).FirstOrDefault(i => i.Id == TopicId);
        if (s is null)
        {
            return RedirectToPage("/404"); 
        }
        TopicData = s;
        return Page(); 
    }

    public IActionResult OnGetAvatar(long? userUid)
    {
        var a = _db.Users.FirstOrDefault(i => i.Id == userUid)?.AvatarUuid;
        if (a is null)
        {
            return NotFound(); 
        }

        var b = _binaryDbContext.Binaries.Find(a);
        if (b is null)
        {
            return NotFound(); 
        }

        return File(b.Content, "image/webp"); 

    }

    public string? Username(long? userUid)
    {
        return _db.Users.FirstOrDefault(i => i.Id == userUid)?.Username; 
    }

    public string? DisplayName(long? userUid) => _db.Users.Find(userUid)?.Username; 
    public Post? GetPost(long? postId)
    {
        if (postId is null)
        {
            return null; 
        }

        return _db.Posts.FirstOrDefault(i => i.Id == postId);
    }

    public bool IsAuthorized() => _db.Users.Find(HttpContext.Session.GetLong("uid")) != null;
    public bool IsAdmin() => HttpContext.Session.GetLong("uid") == 2;

    public IActionResult OnGetTopicDelete()
    {
        var s = _db.Topics.Include(i=>i.Creater).Include(i=>i.Board).Include(i=>i.Posts.OrderBy(i=>i.CreatedAt)).ThenInclude(i=>i.Creator).FirstOrDefault(i => i.Id == TopicId);
        if (s is null)
        {
            return RedirectToPage("/Index");// TODO Badreq page
        }
        _db.Topics.Remove(s);
        _db.SaveChanges();
        return RedirectToPage("/Board" ,new{s.BoardId});
    }

    public IActionResult OnGetPostDelete(long postId)
    {
        var t = _db.Posts.Find(postId);
        if (t is null)
        {
            return BadRequest(); 
        }
        _db.Posts.Remove(t);
        _db.SaveChanges();
        return RedirectToPage("/Topic", new
        {
            t.TopicId
        });
    }
}
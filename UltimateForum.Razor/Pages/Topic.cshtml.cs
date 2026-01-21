using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Db.Models;

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
        return (_db.Users.FirstOrDefault(i=>i.Id == userUid)?.AvatarUuid is null)
            ? NotFound()
            :
            _binaryDbContext.Binaries.FirstOrDefault(i => i.Uuid == _db.Users.FirstOrDefault(i=>i.Id == userUid).AvatarUuid)?.Content is null
                ?
                NotFound()
                : File(_binaryDbContext.Binaries.FirstOrDefault(i => i.Uuid ==  _db.Users.FirstOrDefault(i=>i.Id == userUid).AvatarUuid)?.Content!,
                    "image/webp");
    }

    public string? Username(long? userUid)
    {
        return _db.Users.FirstOrDefault(i => i.Id == userUid)?.Username; 
    }

    public Post? GetPost(long? postId)
    {
        if (postId is null)
        {
            return null; 
        }

        return _db.Posts.FirstOrDefault(i => i.Id == postId);
    }
}
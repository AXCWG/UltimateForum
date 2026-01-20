using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;

namespace UltimateForum.Razor.Pages;

public class Topic(ForumDbContext forumDbContext, BinaryDbContext binaryDbContext) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int TopicId { get; set; }
    public Db.Models.Topic? TopicData { get; set; }
    private readonly ForumDbContext _db = forumDbContext; 
    private readonly BinaryDbContext _binaryDbContext = binaryDbContext;
    public IActionResult OnGet()
    {
        var s = _db.Topics.Include(i=>i.Creater).Include(i=>i.Posts.OrderBy(i=>i.CreatedAt)).ThenInclude(i=>i.Quotting).FirstOrDefault(i => i.Id == TopicId);
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
}
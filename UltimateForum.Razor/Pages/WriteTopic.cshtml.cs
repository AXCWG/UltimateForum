using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Pages.User;

namespace UltimateForum.Razor.Pages;

public class WriteTopic(ForumDbContext forumDbContext, IConfiguration configuration) : PageModel
{
    public readonly IConfiguration Configuration = configuration;
    private readonly ForumDbContext _forumDbContext = forumDbContext;
    [BindProperty(SupportsGet = true)]
    [Required]
    public required long BoardId { get; set; }
    
    [BindProperty]
    [Required(AllowEmptyStrings = false, ErrorMessage = "标题不可为空")]
    public required string Title { get; set; }
    [BindProperty]
    [Required(AllowEmptyStrings = false, ErrorMessage = "内容不可为空")]
    public new required string Content { get; set; }
    
    public IActionResult OnGet()
    {
        if (BoardId is 0 || _forumDbContext.Boards.Find(BoardId) is null)
        {
            return RedirectToPage("/Index");
        }
        
        if (Configuration["AllowAnonymousTopic"]?.ToLowerInvariant() != "true" && (HttpContext.Session.GetLong("uid") is null || _forumDbContext.Users.Any(i=>i.Id == HttpContext.Session.GetLong("uid"))))
        {
            return RedirectToPage("/Index");
        }

        return Page(); 
    }

    public IActionResult OnPostPostTopic()
    {
        var board = _forumDbContext.Boards.Find(BoardId);
        if (board is null)
        {
            return BadRequest(); 
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(); 
        }

        var user = _forumDbContext.Users.Find(HttpContext.Session.GetLong("uid")); 
        if (Configuration["AllowAnonymousTopic"]?.ToLowerInvariant() != "true" && user is null)
        {
            return BadRequest(); 
        }

        var topic = new Db.Models.Topic
        {
            Title = Title,
            Content = Content,
            Creater = user ?? _forumDbContext.Users.Find(1L),
            CreatedOn = DateTime.Now,
            Board = board 
        }; 
        _forumDbContext.Topics.Add(topic);
        _forumDbContext.SaveChanges();
        
        return RedirectToPage("/Topic", new { TopicId = topic.Id });
    }
}
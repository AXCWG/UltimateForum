using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Db.Models;

namespace UltimateForum.Razor.Pages.Shared;

public class RenderQuote(ForumDbContext dbContext, long id) : PageModel
{
    private readonly ForumDbContext _db = dbContext;
    /// <summary>
    /// Use with caution. 
    /// </summary>
    public ForumDbContext Db => _db;

    public long Id { get; set; } = id;
    public void OnGet()
    {
        
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
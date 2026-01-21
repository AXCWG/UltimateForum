using System.ComponentModel.DataAnnotations;
using AXHelper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Db.Models;
using UltimateForum.Razor.Pages.User;

namespace UltimateForum.Razor.Pages;

public class WritePost(ForumDbContext db, IConfiguration config) : PageModel
{
    private readonly ForumDbContext _db = db;
    /// <summary>
    /// Use with caution. 
    /// </summary>
    public ForumDbContext Db => _db;
    private readonly IConfiguration _config = config;
    [BindProperty(SupportsGet = true)]
    public long? TopicId { get; set; }
    public Db.Models.Topic? TopicData { get; set; }
    

    [BindProperty]
    [Required(AllowEmptyStrings =  false, ErrorMessage = "此项不为空")]
    public new string Content { get; set; } = "";
    [BindProperty]
    public string[] AttachmentUuid { get; set; } = [];

    public IActionResult OnGet()
    {
        if (TopicId is null)
        {
            return BadRequest(); 
        }

        if (!_db.Topics.Any(i => i.Id == TopicId))
        {
            return NotFound();
        }

        TopicData = _db.Topics.Include(i=>i.Creater).Include(i=>i.Posts).ThenInclude(i=>i.Creator).First(i => i.Id == TopicId);
        return Page(); 
    }

    public IActionResult OnPostPostPost()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(); 
        }
        #region UserRetrieve

        var u = _db.Users.FirstOrDefault(i => i.Id == HttpContext.Session.GetLong("uid"));
        
        if (u is null && _config["AllowAnonymousPost"] != "True")
        {
            return BadRequest("Anonymous post is not allowed.");
        }

        #endregion

        var firstOrDefault = _db.Topics.FirstOrDefault(i=>i.Id == TopicId);

        _db.Posts.Add(new()
        {
            Content = Content,
            Topic = firstOrDefault ?? throw new InvalidOperationException(),
            AttachmentUuid = [], CreatedAt = DateTime.Now, Creator = u
        });
        _db.SaveChanges();
        return RedirectToPage("/Topic", new{TopicId});
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Pages.User;

namespace UltimateForum.Razor.Pages;

public class Dashboard(ForumDbContext db, AppConfiguration config) : PageModel
{ 
    [Required(AllowEmptyStrings = false, ErrorMessage = "请输入名称")]
    [BindProperty]
    public string? NewName { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "请输入欢迎语")]
    [BindProperty]
    public string? ForumNameIndexWelcomePhrase { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "请输入介绍文")]
    [BindProperty]
    public string? ForumDescription { get; set; }
    
    
    private readonly ForumDbContext _db = db ;
    public readonly AppConfiguration Config = config;
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetLong("uid") != 2)
        {
            return RedirectToPage("/index"); 
        }
        return Page();
    }

    public int TopicCount() => _db.Topics.Count();
    public int UserCount() => _db.Users.Count();
    public int PostCount() => _db.Posts.Count();

    public async Task<IActionResult> OnPostEditName()
    {
        if (ModelState.GetFieldValidationState("NewName") != ModelValidationState.Valid)
        {
            return BadRequest(); 
        }
        await Config.SetValueAsync("ForumName", NewName ?? throw new NullReferenceException());
        return RedirectToPage("/Dashboard"); 
    }

    public async Task<IActionResult> OnPostSaveIndexBlock()
    {
        if (ModelState.GetFieldValidationState("ForumNameIndexWelcomePhrase") != ModelValidationState.Valid)
        {
            return BadRequest(); 
        }

        if (ModelState.GetFieldValidationState("ForumDescription") != ModelValidationState.Valid)
        {
            return BadRequest(); 
        }
        await Config.SetValueAsync("ForumNameIndexWelcomePhrase",
            ForumNameIndexWelcomePhrase ?? throw new NullReferenceException());
        await Config.SetValueAsync("ForumDescription", ForumDescription ?? throw new NullReferenceException());
        return RedirectToPage("/Dashboard"); 
    }
    
    
}
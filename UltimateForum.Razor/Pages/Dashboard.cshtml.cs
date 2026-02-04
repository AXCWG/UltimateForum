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

    public class Options : IAppConfigurationProperties
    {
        public bool AllowUserCreateBoard { get; set; }
        public bool AllowAnonymousPost { get; set; }
        public bool AllowAnonymousTopic { get; set; }
        public bool AllowAnonymousBoard { get; set; }
        public bool ShowCreateBoardWhenRequirementNotMet { get; set; }
        public bool ShowCreateTopicWhenRequirementNotMet { get; set; }
        public bool ShowCreatePostWhenRequirementNotMet { get; set; }
    }
    [BindProperty]
    public Options Option { get; set; } = new();
    
    private readonly ForumDbContext _db = db ;
    public readonly AppConfiguration Config = config;
    public IActionResult OnGet()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/User/Login"); 
        }
        return Page();
    }

    public int TopicCount() => _db.Topics.Count();
    public int UserCount() => _db.Users.Count();
    public int PostCount() => _db.Posts.Count();
    public string? GetUseBoardCreation() => Config["UseBoardCreation"];
    public string? GetAllowUserCreateBoard() => Config["AllowUserCreateBoard"];
    public string? GetAllowAnonymousPost()=> Config["AllowAnonymousPost"];
    public string? GetAllowAnonymousTopic() => Config["AllowAnonymousTopic"];
    public string? GetAllowAnonymousBoard()=> Config["AllowAnonymousBoard"];

    public async Task<IActionResult> OnPostEditName()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/User/Login");
        }
        if (ModelState.GetFieldValidationState("NewName") != ModelValidationState.Valid)
        {
            return BadRequest(); 
        }
        await Config.SetValueAsync("ForumName", NewName ?? throw new NullReferenceException());
        return RedirectToPage("/Dashboard"); 
    }

    public async Task<IActionResult> OnPostSaveIndexBlock()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/User/Login");
        }
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

    public IActionResult OnPostSaveTogglesBlock()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/User/Login"); 
        }
        foreach (var i in typeof(IAppConfigurationProperties).GetProperties())
        {
            typeof(IAppConfigurationProperties).GetProperty(i.Name)!.SetValue(Config, i.GetValue(Option));
            
        }
        return RedirectToPage("/Dashboard");
    }

    public bool IsAdmin() => HttpContext.Session.GetLong("uid") == 2; 
}
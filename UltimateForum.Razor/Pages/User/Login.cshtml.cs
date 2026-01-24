using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UltimateForum.Razor.Db;

namespace UltimateForum.Razor.Pages.User;

public class Login(ForumDbContext dbContext) : PageModel
{
    private readonly ForumDbContext _db = dbContext; 
    [BindProperty]
    [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不可为空")]
    public required string Username { get; set; }
    [BindProperty]
    [Required(AllowEmptyStrings = false, ErrorMessage = "密码不可为空")]
    public required string Password { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? WhereYouAreFrom { get; set; }
    public void OnGet()
    {
        Console.WriteLine(WhereYouAreFrom);
    }

    public IActionResult OnPostLogin()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(); 
        }

        var user = _db.Users.FirstOrDefault(i => i.Username == Username);
        if (user is null)
        {
            ModelState.AddModelError("Username", "用户名或密码错误");
            return Page(); 
        }
        HttpContext.Session.SetString("uid", user.Id.ToString());
        return WhereYouAreFrom is null ?  RedirectToPage("/Index") : Redirect(WhereYouAreFrom); 
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;

namespace UltimateForum.Razor.Pages.User;

public class Home(ForumDbContext forumDbContext) : PageModel
{
    private readonly ForumDbContext _dbContext = forumDbContext;
    public new UltimateForum.Db.Models.User User { get; set; } = null!;
    [BindProperty(SupportsGet = true)]
    public bool Edit { get; set; }

    public class EditProfileModel
    {
        public string? Email { get; set; }
    }
    [BindProperty]
    public EditProfileModel EditProfile { get; set; } = null!;
    public IActionResult OnGet()
    {
        var u = _dbContext.Users.Include(i=>i.CreatedPosts).Include(i=>i.CreatedTopics).FirstOrDefault(i => i.Id == HttpContext.Session.GetLong("uid"));
        if (u is null)
        {
            return RedirectToPage("/Index");
        }

        User = u;
        if (Edit)
        {
            EditProfile = new()
            {
                Email = User.Email
            };
        }
        return Page(); 
    }

    public IActionResult OnGetLogout()
    {
        HttpContext.Session.Remove("uid");
        return RedirectToPage("/Index");
    }

    public IActionResult OnPostEditProfile()
    {
        _dbContext.Users.Where(i=>i.Id == HttpContext.Session.GetLong("uid")).ExecuteUpdate(i => i.SetProperty(u=>u.Email, EditProfile.Email));
        return RedirectToPage("/User/Home"); 
    }

    public IActionResult OnGetDeleteAccount()
    {
        var u = _dbContext.Users.Find(HttpContext.Session.GetLong("uid"));
        if (u is null)
        {
            return BadRequest(); 
        }
        _dbContext.Users.Remove(u);
        _dbContext.SaveChanges();
        HttpContext.Session.Remove("uid");
        return RedirectToPage("/Index");
    }
}
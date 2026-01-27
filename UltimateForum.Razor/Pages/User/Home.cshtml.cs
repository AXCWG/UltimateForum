using System.ComponentModel.DataAnnotations;
using AXHelper.Extensions;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;

namespace UltimateForum.Razor.Pages.User;

[DisableRequestSizeLimit]
public class Home(ForumDbContext forumDbContext, BinaryDbContext binaryDbContext ): PageModel
{
    private readonly ForumDbContext _dbContext = forumDbContext;
    private readonly BinaryDbContext _binaryDbContext = binaryDbContext;

    
    public new UltimateForum.Db.Models.User User { get; set; } = null!;
    [BindProperty(SupportsGet = true)]
    public bool EditProfileFlag { get; set; }
    [BindProperty(SupportsGet = true)]
    public bool EditDescFlag { get; set; }

    public class EditProfileModel
    {
        public string? Email { get; set; }
    }
    [BindProperty]
    public EditProfileModel EditProfile { get; set; } = null!;

    public class AvatarUploadModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public IFormFile Profile { get; set; } = null!;
    }
    [BindProperty]
    public AvatarUploadModel AvatarUpload { get; set; } = null!;

    public class EditDescModel
    { 
        public string? Description { get; set; }
    }

    [BindProperty] 
    public EditDescModel EditDesc { get; set; } = null!;
    
    public IActionResult OnGet()
    {
        var u = _dbContext.Users.Include(i=>i.CreatedPosts).Include(i=>i.CreatedTopics).FirstOrDefault(i => i.Id == HttpContext.Session.GetLong("uid"));
        if (u is null)
        {
            return RedirectToPage("/Index");
        }
        
        User = u;
        if (EditProfileFlag)
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
    public IActionResult OnPostUploadAvatar()
    {
        var original = _dbContext.Users.FirstOrDefault(i => i.Id == HttpContext.Session.GetLong("uid"))?.AvatarUuid;
        if (original is not null)
        {
            var oB = _binaryDbContext.Binaries.Find(original);
            if (oB is not null)
            {
                _binaryDbContext.Remove(oB);
            }
        }
        var guid = Guid.NewGuid().ToString();
        _binaryDbContext.Binaries.Add(new()
        {
            Uuid = guid, Content = AvatarUpload.Profile.ReadToByteArray()
        });
        _dbContext.Users.Find(HttpContext.Session.GetLong("uid"))?.AvatarUuid = guid; 
        _binaryDbContext.SaveChanges();
        _dbContext.SaveChanges();
        return RedirectToPage("/User/Home");
    }

    public IActionResult OnGetGetAvatar()
    {
        
        var c = _binaryDbContext.Binaries.Find(_dbContext.Users.FirstOrDefault(i=>i.Id == HttpContext.Session.GetLong("uid"))?.AvatarUuid)?.Content;
        if (c is null)
        {
            return NotFound(); 
        }
        return File(c, "image/webp");
    }

    public IActionResult OnPostEditDescHandler()
    {
        var sanitizer = new HtmlSanitizer(); 
        _dbContext.Users.Where(i => i.Id == HttpContext.Session.GetLong("uid"))
            .ExecuteUpdate(i => i.SetProperty(p => p.Description, EditDesc.Description is null ? null :sanitizer.Sanitize(EditDesc.Description)));
        return RedirectToPage("/User/Home");
    }
    
}
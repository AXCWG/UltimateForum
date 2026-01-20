using System.ComponentModel.DataAnnotations;
using AXHelper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UltimateForum.Razor.Db;

namespace UltimateForum.Razor.Pages.User;

public class ChangePassword(ForumDbContext context) : PageModel
{
    private readonly ForumDbContext _db = context;
    public class PasswordChangeModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "此处不能为空")]
        public required string OldPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "此处不能为空")]
        public required string NewPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "此处不能为空")]
        [Compare(nameof(NewPassword), ErrorMessage = "确认密码必须一致")]
        public required string ConfirmPassword { get; set; }
    }
    [BindProperty]
    public PasswordChangeModel? PasswordChange { get; set; }
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("uid") is null)
        {
            return RedirectToPage("/Index");
            
        }
        return Page();
    }

    public IActionResult OnPostChangePassword()
    {
        if (!ModelState.IsValid) return BadRequest("输入不合法");
        _db.Users.Find(HttpContext.Session.GetLong("uid"))?.Password = PasswordChange?.NewPassword.ToSha256String() ?? throw new InvalidOperationException();
        _db.SaveChanges();
        HttpContext.Session.Remove("uid");
        return RedirectToPage("/Index");
    }
}

public static class SesisonExtension
{
    extension(ISession session)
    {
        public long GetLong(string key)
        {
            return long.Parse(session.GetString(key) ?? throw new InvalidOperationException());
        }
    }
}
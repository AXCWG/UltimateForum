using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using AXHelper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db;

namespace UltimateForum.Razor.Pages.User;

public class Register(ForumDbContext dbContext) : PageModel
{
    // TODO Register confirmation
    private readonly ForumDbContext _dbContext = dbContext;
    
    public class RegisterFormModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不可为空")]
        public required string Username { get; set; }    
        public string? Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不可为空")]
        public required string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "请确认密码")]
        [Compare(nameof(Password), ErrorMessage = "密码不一致")]
        public required string ConfirmPassword { get; set; }
    }
    [BindProperty]
    public RegisterFormModel? Form { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? WhereYouAreFrom { get; set; }
    public void OnGet()
    {
        
    }

    public IActionResult OnPostRegister()
    {
        if (!ModelState.IsValid || Form is null)
        {
            ModelState.AddModelError("Username", "数据验证失败");
            return Page(); 
        }

        if (_dbContext.Users.Any(i => i.Username == Form.Username))
        {
            ModelState.AddModelError("Username", "用户名已存在");
            return Page();
        }
        try
        {
            
            _dbContext.Users.Add(new()
            {
                Username = Form.Username,
                Password = Form.Password.ToSha256String(),
                Joined = DateTime.Now,
                Email =  Form.Email,
                Op = false,
            });
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ModelState.AddModelError("Username", "服务器内部错误，请联系管理员");
            return Page(); 
        }

        var id = _dbContext.Users.FirstOrDefault(i => i.Username == Form.Username)?.Id;
        if (id is null)
        {
            ModelState.AddModelError("Username", "注册失败，请联系管理员");
            return Page(); 
        }
        HttpContext.Session.SetString("uid", id.Value.ToString());
        return WhereYouAreFrom is null ? RedirectToPage("/Index") : Redirect(WhereYouAreFrom);
    }
}
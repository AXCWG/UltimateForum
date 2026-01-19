using System.ComponentModel.DataAnnotations;
using AXHelper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UltimateForum.Db;

namespace UltimateForum.Razor.Pages;

public class IndexModel(ForumDbContext context, BinaryDbContext binaryDbContext) : PageModel
{
    private readonly ForumDbContext _db = context;
    private readonly BinaryDbContext _binaryDbContext = binaryDbContext;

    
    [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名为必填项")]
        public required string Username { get; set; }
    [BindProperty]
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码为必填项")]
        public required string Password { get; set; }
    
    public void OnGet()
    {
        if(HttpContext.Session.GetString("uid") != null)
        {
            var u = _db.Users.FirstOrDefault(i=>i.Id == long.Parse(HttpContext.Session.GetString("uid")));
            if (u is null)
            {
                HttpContext.Session.Remove("uid");
                RedirectToPage("/Index");
                return; 
            }
            Username = u.Username; 
            
        }
    }

    public IActionResult OnPostLogin()
    {
        if (!ModelState.IsValid) return BadRequest("输入不合法");
       
      var u = _db.Users.FirstOrDefault(i=>i.Username == Username && i.Password == Password.ToSha256String());
      if (u == null)
      {
          ModelState.AddModelError( "Username", "用户名或密码错误");
          return Page(); 
      }

      HttpContext.Session.SetString("uid", u.Id.ToString());
        return RedirectToPage("/Index");
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove("uid");
        return RedirectToPage("/Index"); 
    }

   

    
}
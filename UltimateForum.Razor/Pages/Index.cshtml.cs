using System.ComponentModel.DataAnnotations;
using AXHelper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UltimateForum.Db;
using UltimateForum.Db.Models;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Db.Models;

namespace UltimateForum.Razor.Pages;

public class IndexModel(ForumDbContext context, BinaryDbContext binaryDbContext, IConfiguration configuration) : PageModel
{
    private readonly ForumDbContext _db = context;
    private readonly BinaryDbContext _binaryDbContext = binaryDbContext;
    public readonly IConfiguration Configuration = configuration;
    public List<Db.Models.Board> Boards = []; 

    
    [BindProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名为必填项")]
        public required string Username { get; set; }
    [BindProperty]
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码为必填项")]
        public required string Password { get; set; }
    
    public IActionResult OnGet()
    {
        if (Configuration["AllowUserCreateBoard"] == "True")
        {
            Boards = _db.Boards
                .OrderByDescending(i => i.Created)
                .ToList();
        }
        else
        {
            Boards = _db.Boards
                .OrderBy(i => i.Order).ToList();
        }
        if(HttpContext.Session.GetString("uid") != null)
        {
            var u = _db.Users.FirstOrDefault(i=>i.Id == long.Parse(HttpContext.Session.GetString("uid") ?? string.Empty));
            if (u is null)
            {
                HttpContext.Session.Remove("uid");
                return RedirectToPage("/Index");
            }
            Username = u.Username; 
            
        }
        return Page();
    }

    public int GetTopicCount(long boardId)
    {
        return _db.Topics.Count(i => i.BoardId == boardId);
    }

    public int GetPostCount(long boardId)
    {
        return _db.Topics.Where(i=>i.BoardId == boardId).Include(i => i.Posts).Sum(i => i.Posts.Count);
    }

    public Post? GetLatestPost(long boardId)
    {
        return _db.Posts.Include(i=>i.Topic).Where(i=>i.Topic.BoardId == boardId).Include(i=>i.Creator).OrderByDescending(i=>i.CreatedAt).FirstOrDefault() ?? null;
    }
    public IActionResult OnPostLogin()
    {
        if (!ModelState.IsValid) return BadRequest("输入不合法");
       
      var u = _db.Users.FirstOrDefault(i=>i.Username == Username && i.Password == Password.ToSha256String());
      if (u == null)
      {
          ModelState.AddModelError( "Username", "用户名或密码错误");
          return OnGet(); 
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
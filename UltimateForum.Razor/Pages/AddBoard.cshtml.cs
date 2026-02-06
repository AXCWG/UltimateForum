using System.ComponentModel.DataAnnotations;
using AXHelper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Pages.User;

namespace UltimateForum.Razor.Pages;

public class AddBoard(ForumDbContext dbContext, AppConfiguration config) : PageModel
{
    private readonly ForumDbContext _db = dbContext;
    public class AddBoardModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "名称不可为空")]
        public required string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "简介不可为空")]
        public required string Description { get; set; }
    }
    
    [BindProperty] public AddBoardModel AddBoardInst { get; set; } = null!; 
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetLong("uid") is null && !config["AllowAnonymousBoard"].ParseBool())
        {
            return RedirectToPage("/User/Login", new
            {
                WhereYouAreFrom="/AddBoard"
            }); 
        }

        return Page(); 
    }

    public IActionResult OnPostAddBoard()
    {
        if (ModelState.IsValid == false)
        {
            ModelState.AddModelError("AddBoardInst.Name", "未知错误");
            return Page();
        }

        var l = _db.Boards.OrderBy(i=>i.Id).Last();
        var s = new Db.Models.Board
        {
            Order = Random.Shared.NextSingle(l.Order, l.Order+1),
            Name = AddBoardInst.Name,
            Description = AddBoardInst.Description,
            Created = DateTime.Now,
            CreatedBy = _db.Users.Find(HttpContext.Session.GetLong("uid")) ?? _db.Users.Find(1L),

        }; 
        _db.Boards.Add(s); _db.SaveChanges();
        
        return RedirectToPage("/Board", new{BoardId=s.Id});
    }
}
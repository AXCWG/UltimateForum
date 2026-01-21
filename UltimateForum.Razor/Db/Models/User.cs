using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Razor.Db.Models;

namespace UltimateForum.Db.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Username), IsUnique = true)]
public class User
{
  
    public long Id { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }
    public required string Password { get; set;  }
    public string? Description { get; set; }
    public string? AvatarUuid { get; set; }
   
    public ICollection<BoardUserOrganizer> BoardUserOrganizers { get; set; }
    
    // Creator
    public ICollection<Topic> CreatedTopics { get; set; }
    public ICollection<Post> CreatedPosts { get; set;  }
    public ICollection<BoardGroup> CreatedBoardGroups { get; set; }
    public required DateTime Joined { get; set; }
    public required bool Op { get; set; }
}
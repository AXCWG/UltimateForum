using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
    public ICollection<Board> Boards { get; set; }
    public ICollection<Post> Posts { get; set; }
}
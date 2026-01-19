using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Db.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Title))]
public class Post
{
    public int Id { get; set; }
    public required string Title { get; set;  }
    public required string Content { get; set;  }
    public Board Parent { get; set; } = null!;
    [ForeignKey(nameof(Board))]
    public long ParentId { get; set; }
    public User? Creater { get; set; }
    [ForeignKey(nameof(User))]
    public long? CreaterId { get; set; }
    public required DateTime CreatedOn { get; set; }
}
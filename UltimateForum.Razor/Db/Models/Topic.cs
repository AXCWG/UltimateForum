using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;

namespace UltimateForum.Razor.Db.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Title))]
public class Topic
{
    public long Id { get; set; }
    public required string Title { get; set;  }
    public required string Content { get; set;  }
    public Board Board { get; set; } = null!;
    public long BoardId { get; set; }
    public required User? Creater { get; set; }
    public long? CreaterId { get; set; }
    public required DateTime CreatedOn { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
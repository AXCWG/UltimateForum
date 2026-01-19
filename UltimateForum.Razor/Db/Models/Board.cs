using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Db.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Name))]
public class Board
{
    public long Id { get; set;  }
    public required int Order { get; set;  }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<Post> Posts { get; set; } = [];
    
    public required DateTime Created { get; set; }
    public User? CreatedBy { get; set; }
    [ForeignKey(nameof(User))]
    public long? CreatedById { get; set; }
    
}
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;

namespace UltimateForum.Razor.Db.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Name))]
public class BoardGroup
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Board> Boards { get; set; } = new List<Board>(); 
    public required DateTime CreatedAt { get; set; }
    public User? CreatedBy { get; set; }
    public long CreatedById { get; set;  }
}
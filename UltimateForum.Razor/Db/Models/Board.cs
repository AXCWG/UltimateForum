using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;

namespace UltimateForum.Razor.Db.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Name))]
public class Board
{
    public long Id { get; set;  }
    public required int Order { get; set;  }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<Topic> Topics { get; set; }
    
    public required DateTime Created { get; set; }
    
    public ICollection<BoardUserOrganizer> BoardUserOrganizers { get; set; }
    
    public BoardGroup? BoardGroup { get; set; }
    [ForeignKey(nameof(BoardGroup))]
    public long? BoardGroupId { get; set; }

}
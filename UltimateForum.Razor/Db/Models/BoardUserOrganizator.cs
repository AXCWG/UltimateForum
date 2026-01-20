using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;

namespace UltimateForum.Razor.Db.Models;

[PrimaryKey(nameof(Uuid))]
[Index(nameof(DesignatedId))]
[Index(nameof(BoardId))]
public class BoardUserOrganizer
{
    public enum Role
    {
        Admin, 
        Moderator, 
        Banned, 
    }
    [MaxLength(36)]
    public required string Uuid { get; set; }
    
    public required User Designated { get; set; }
    public long DesignatedId { get; set; } 
    public required Board Board { get; set; }
    public long BoardId { get; set; }
    public required DateTime Since { get; set; }
    public required Role Status { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;

namespace UltimateForum.Razor.Db.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(Content))]
public class Post
{
    
    public long Id { get; set; }

    
    public required string Content { get; set; }
    public required Topic Topic { get; set; }
    public long TopicId { get; set; }

   
    public User? Creator { get; set; }
    public long CreatorId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required List<string> AttachmentUuid { get; set; }
}
using Microsoft.EntityFrameworkCore;

namespace UltimateForum.Razor.Db.Models;

[PrimaryKey(nameof(Uuid))]
public class Binary
{
    public required string Uuid { get; set; }
    public required byte[] Content { get; set;  }
}
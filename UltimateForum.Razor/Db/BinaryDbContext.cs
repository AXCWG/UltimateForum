using AXHelper.Extensions;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;
using UltimateForum.Razor.Db.Models;

namespace UltimateForum.Razor.Db;

public class BinaryDbContext : DbContext
{
    public DbSet<Binary> Binaries { get; set; }
    public BinaryDbContext()
    {
    }
    public BinaryDbContext(DbContextOptions<BinaryDbContext> options) : base(options)
    {
    }
    
}
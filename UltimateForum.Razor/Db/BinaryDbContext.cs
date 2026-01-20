using AXHelper.Extensions;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;
using UltimateForum.Razor.Db.Models;

namespace UltimateForum.Razor.Db;

public class BinaryDbContext : DbContext
{
    public DbSet<Binary> Binaries { get; set; }
    private readonly string _str; 
    public BinaryDbContext(string str)
    {
        _str = str; 
    }
    public BinaryDbContext(DbContextOptions<BinaryDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _str.CreateDirectoryOfDataSource();
        optionsBuilder.UseSqlite(_str);
    }
}
using AXHelper.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;

namespace UltimateForum.Db;

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
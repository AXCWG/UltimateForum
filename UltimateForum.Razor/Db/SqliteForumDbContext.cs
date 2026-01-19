using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;

namespace UltimateForum.Db;

public class ForumDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Board> Boards { get; set; }
    private readonly string _str;
    private readonly DbType _type; 
    public enum DbType
    {
        Mysql, 
        Sqlite
    }
    public ForumDbContext(DbContextOptions<ForumDbContext> options)
        : base(options)
    {
        
    }

    public ForumDbContext(string str, DbType type )
    {
        _str = str; 
        _type =  type;
        }
    
}
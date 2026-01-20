using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;
using UltimateForum.Razor.Db.Models;

namespace UltimateForum.Razor.Db;

public class ForumDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<BoardGroup> BoardGroups { get; set; }
    public DbSet<BoardUserOrganizer> BoardUserOrganizers { get; set; }
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Topic>().HasOne(i => i.Board).WithMany(i => i.Topics).HasForeignKey(i => i.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Topic>().HasOne(i => i.Creater).WithMany(i => i.CreatedTopics)
            .HasForeignKey(i => i.CreaterId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Post>().HasOne(i => i.Topic).WithMany(i => i.Posts).HasForeignKey(i => i.TopicId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Post>().HasOne(i => i.Creator).WithMany(i => i.CreatedPosts).HasForeignKey(i => i.CreatorId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Post>().HasMany(i => i.Quotting).WithOne().HasPrincipalKey(i=>i.Id).OnDelete(DeleteBehavior.ClientSetNull); 
        modelBuilder.Entity<BoardUserOrganizer>().HasOne(i => i.Board).WithMany(i => i.BoardUserOrganizers)
            .HasForeignKey(i => i.BoardId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<BoardUserOrganizer>().HasOne(i => i.Designated).WithMany(i => i.BoardUserOrganizers)
            .HasForeignKey(i => i.DesignatedId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<BoardGroup>().HasOne(i => i.CreatedBy).WithMany(i => i.CreatedBoardGroups)
            .HasForeignKey(i => i.CreatedById).OnDelete(DeleteBehavior.ClientSetNull);
        modelBuilder.Entity<Board>().HasOne(i=>i.BoardGroup).WithMany(i=>i.Boards).HasForeignKey(i=>i.BoardGroupId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
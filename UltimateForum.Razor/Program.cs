using AXHelper.Extensions;
using AXHelper.Helpers;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db;
using UltimateForum.Db.Models;
using UltimateForum.Razor.Db;
using UltimateForum.Razor.Db.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromDays(7);
});
builder.Services.AddMemoryCache();
switch (builder.Configuration["DbType"])
{
    case "sqlite":
        builder.Services.AddDbContext<ForumDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
        break;
    case "mysql":
        throw new NotImplementedException();
    default:
        throw new Exception("No valid database type configured in appsettings.json");
}
using (var db = new BinaryDbContext(builder.Configuration.GetConnectionString("BinaryConnection") ?? throw new Exception("Not initted. ")))
{
    db.Database.EnsureCreated();
}
builder.Services.AddDbContext<BinaryDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("BinaryConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession(); 
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
    db.Database.Migrate(); 
    if (!File.Exists("INIT"))
    {
        var password = StringHelper.RandomString(8);
    
        var user = new User
        {
            Username = "Admin", Password = password.ToSha256String(), Op = true
        }; 
        db.Users.Add(user);
        var newBoardGroup = new BoardGroup
        {
            Name = "默认板块",
            CreatedAt = DateTime.Now, CreatedBy = user
        };
        db.BoardGroups.Add(newBoardGroup);
        
        
        var newBoard = new Board
        {
            Name = "综合",
            Description = "综合版块",
            Created = DateTime.Now,
            Order = 0,
            BoardGroup = newBoardGroup
        };
        db.Boards.Add(newBoard);

        db.BoardUserOrganizers.Add(new()
        {
            Board = newBoard, Designated = user, Uuid = Guid.NewGuid().ToString(), Since = DateTime.Now,
            Status = BoardUserOrganizer.Role.Admin
        });
        var topic = new Topic()
        {
            Title = "测试贴",
            Content = "欢迎来到Ultimate Forum！\n测试贴的内容",
            Board = newBoard,
            CreatedOn = DateTime.Now, Creater = user
        };
        db.Topics.Add(topic);
        var post = new Post()
        {
            Content = "测试回复呦呦", Creator = user, Topic = topic, CreatedAt = DateTime.Now, AttachmentUuid = []
        };
        db.Posts.Add(post);
        db.Posts.Add(new()
        {
            Content = "测试回复：测试回复呦呦", Creator = user, Topic = topic, CreatedAt = DateTime.Now, AttachmentUuid = [],
            Quotting = [post]
        });
        db.SaveChanges();
        
        File.WriteAllText("INIT", password);

    }
}



app.Run();
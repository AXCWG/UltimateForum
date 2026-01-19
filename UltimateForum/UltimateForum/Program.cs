using AXHelper.StringExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using UltimateForum.Client.Pages;
using UltimateForum.Components;
using UltimateForum.Db;
using UltimateForum.Db.Models;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddFluentUIComponents();
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(UltimateForum.Client._Imports).Assembly);
if (!File.Exists("INIT"))
{
    var password = StringExtensions.RandomString(8);
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
        db.Database.EnsureCreated();
        var user = new User
        {
            Username = "Admin", Password = password.ToSha256String()
        }; 
        db.Users.Add(user);
        var newBoard = new Board()
        {
            Name = "综合",
            Description = "综合版块",
            Created = DateTime.Now, Order = 0, CreatedBy = user
        };
        db.Boards.Add(newBoard);
        db.Posts.Add(new()
        {
            Title = "测试贴",
            Content = "欢迎来到Ultimate Forum！\n测试贴的内容",
            Parent = newBoard,
            CreatedOn = DateTime.Now, Creater = user
        });
        db.SaveChanges();

    }
    File.WriteAllText("INIT", password);
}
app.Run();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IPlayerService, PlayerService>();
builder.Services.AddScoped<ITeamService, TeamService>();

var app = builder.Build();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Team}/{action=Index}/{id?}");
app.MapControllers();


app.Run();

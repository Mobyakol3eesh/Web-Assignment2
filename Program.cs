using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var baseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connBuilder = new MySqlConnector.MySqlConnectionStringBuilder(baseConnectionString ?? "")
{
	Password = password
};

var connectionString = connBuilder.ConnectionString;

builder.Services.AddDbContext<TunaLeagueContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();



var app = builder.Build();





app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Team}/{action=Index}/{id?}");
app.MapControllers();


app.Run();

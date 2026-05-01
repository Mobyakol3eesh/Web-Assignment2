var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var connectionString = AppConfiguration.BuildConnectionString(builder.Configuration);
var jwtConfig = AppConfiguration.LoadJwtConfig(builder.Configuration);

builder.Services.AddApplicationServices(connectionString, jwtConfig);

var app = builder.Build();

app.UseApplicationMiddleware();


app.Run();

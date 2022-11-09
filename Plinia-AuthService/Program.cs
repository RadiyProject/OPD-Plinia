using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Plinia_AuthService.DB;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connection = configuration.GetConnectionString("MySQLConnection");
builder.Services.AddDbContext<MySqlDbContext>(optionsBuilder =>
    optionsBuilder
        .UseMySql(new MySqlConnection(connection),
            new MySqlServerVersion(MySqlServerVersion.LatestSupportedServerVersion))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors());
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
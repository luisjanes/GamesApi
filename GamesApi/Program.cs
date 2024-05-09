using GamesApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GamesDataContext>(
    options =>
        options.UseSqlServer(connectionString));
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapGet("/", () => "Hello World! Este é um teste para a Ploomes :)");

app.Run();

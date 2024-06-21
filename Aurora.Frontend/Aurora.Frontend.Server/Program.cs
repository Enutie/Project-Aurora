using Aurora.Application.Interfaces;
using Aurora.Application.Services;
using Aurora.Infrastructure.Interfaces;
using Aurora.Infrastructure.Persistence;
using Aurora.Server.Models;
using Aurora.Shared.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<ICardConverter, CardConverter>();
builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSingleton<IGameQueryService, GameQueryService>();
builder.Services.AddSingleton<IGameStorage, InMemoryGameStorage>();
builder.Services.AddSingleton<IPlayerActionService, PlayerActionService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

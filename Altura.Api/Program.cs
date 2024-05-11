using Altura.Application.Interfaces;
using Altura.Application.Services;
using Altura.Infrastructure.Apis;
using Altura.Infrastructure.Apis.Models;
using Altura.Infrastructure.ExternalServices;
using Altura.Infrastructure.Interfaces;
using Altura.Infrastructure.Readers;
using TrelloDotNet.AutomationEngine.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITenderService, TenderService>();
builder.Services.AddScoped<ITenderParser, TenderParser>();
builder.Services.AddScoped<ITrelloApi, TrelloApi>();
builder.Services.AddScoped<ITrelloTenderService, TrelloTenderService>();
builder.Services.AddScoped<ITrelloBoard, TrelloBoard>();
builder.Services.AddScoped<ITrelloList, TrelloList>();
builder.Services.AddScoped<ITrelloCard, TrelloCard>();
builder.Services.AddScoped<ITrelloCustomFields, TrelloCustomFields>();

builder.Services.Configure<TrelloConfiguration>(builder.Configuration.GetSection("Trello"));

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

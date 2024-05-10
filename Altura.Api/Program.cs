using Altura.Application.Interfaces;
using Altura.Application.Services;
using Altura.Infrastructure.Apis;
using Altura.Infrastructure.Interfaces;
using Altura.Infrastructure.Readers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITenderProcessor, TenderProcessor>();
builder.Services.AddScoped<ITenderParser, TenderParser>();
builder.Services.AddScoped<ITrelloApi, TrelloApi>();
builder.Services.AddScoped<ITrelloIntegration, TrelloIntegration>();
builder.Services.AddScoped<ITrelloBoard, TrelloBoard>();
builder.Services.AddScoped<ITrelloList, TrelloList>();
builder.Services.AddScoped<ITrelloCard, TrelloCard>();
builder.Services.AddScoped<ITrelloCustomFields, TrelloCustomFields>();
builder.Services.AddScoped<ITrelloIntegration, TrelloIntegration>();

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

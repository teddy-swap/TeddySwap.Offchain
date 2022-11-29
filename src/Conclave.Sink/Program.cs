using System.Text.Json;
using Conclave.Sink.Data;
using Conclave.Sink.Extensions;
using Conclave.Sink.Models;
using Conclave.Sink.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<ConclaveSinkDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConclaveSink"))
);
builder.Services.Configure<ConclaveSinkSettings>(options => builder.Configuration.GetSection("ConclaveSinkSettings").Bind(options));
builder.Services.AddSingleton<CardanoService>();
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

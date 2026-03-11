using ConsultorioApi.Data;
using ConsultorioApi.Models;
using ConsultorioApi.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ConsultorioApi.Services.CpfService>();
builder.Services.AddScoped<ConsultorioApi.Services.EmailService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();


    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

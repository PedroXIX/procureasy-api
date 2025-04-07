using Procureasy.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProcurEasyContext>(options =>
   // options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); //método para conectar ao database definido no appsettings
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotebookConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
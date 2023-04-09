using FlutterApi.Data;
using FlutterApi.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();  

//builder.Services.AddDbContext<FlutterApiDB>(options => options.UseInMemoryDatabase("FlutterDb"));
builder.Services.AddDbContext<FlutterApiDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FlutterApiConnectionString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<SignalrHub>("/signalrHub");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
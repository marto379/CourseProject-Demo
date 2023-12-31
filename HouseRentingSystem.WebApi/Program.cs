using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<HouseRentingDbContext>(opt =>
    opt.UseSqlServer(connectionString));


builder.Services.AddApplicationServices(typeof(IHouseService));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(setup =>
{
    setup.AddPolicy("HouseRentingSystem", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("https://localhost:7296")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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

app.UseCors("HouseRentingSystem");

app.Run();

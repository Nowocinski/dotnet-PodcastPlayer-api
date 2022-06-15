using Microsoft.EntityFrameworkCore;
using WebApplication.Core.Context;
using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.Repositories;
using WebApplication.Infrastructure.Services.User;
using WebApplication.Infrastructure.Services.User.JwtToken;
using WebApplication.Infrastructure.Settings;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtHandler, JwtHandler>();
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"), b => b.MigrationsAssembly("PodcastPlayer-API"));
});
//var aaa = builder.Configuration.GetRequiredSection("Jwt").Get<JwtSettings>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using RepairOpsApi.BusinessLogic;
using RepairOpsApi.BusinessLogic.Interfaces;
using RepairOpsApi.DataAccess;
using RepairOpsApi.DataAccess.Repository;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;
using RepairOpsApi.Helpers;
using RepairOpsApi.Helpers.Interfaces;

namespace RepairOpsApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        
        var secretKey = builder.Configuration["Jwt:SecretKey"];
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];

        builder.Services.AddSingleton(new JwtHandler(secretKey, issuer, audience));

        builder.Services.AddDbContext<RepairOpsApiContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
        });
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserLogic, UserLogic>();
        
        builder.Services.AddScoped<ICaseRepository, CaseRepository>();
        builder.Services.AddScoped<IChatRepository, ChatRepository>();
        builder.Services.AddScoped<IPasswordEncrypter, PasswordEncrypter>();
        builder.Services.AddScoped<INotesRepository, NotesRepository>();
        

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddSignalR();

        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
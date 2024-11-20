using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using RepairOpsApi.BusinessLogic;
using RepairOpsApi.BusinessLogic.Interfaces;
using RepairOpsApi.DataAccess;
using RepairOpsApi.DataAccess.Repository;
using RepairOpsApi.DataAccess.Repository.Interfaces;
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
        builder.Services.AddDbContext<RepairOpsApiContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
        });
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserLogic, UserLogic>();
        
        builder.Services.AddScoped<ICaseRepository, CaseRepository>();
        
        builder.Services.AddScoped<IPasswordEncrypter, PasswordEncrypter>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
    }
}
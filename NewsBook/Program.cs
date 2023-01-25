using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NewsBook.Repository;
using IdentityServer3.Core.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewsBook.Authorization;
using System.Text.Json.Serialization;
using NewsBook.Helpers;
using NewsBook.ModelDTO;

namespace NewsBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors();
            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("DbNews"));
            // configure strongly typed settings object
            builder.Services.Configure<AppSettingsDTO>(builder.Configuration.GetSection("appsettings"));

            builder.Services.AddScoped<INewsRepository, NewsRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IFavouriteNewsRespository, FavouriteNewsRespository>();
            builder.Services.AddScoped<IJwtUtils, JwtUtils>();
         
            //builder.Services.AddScoped<IJwtUtils, JWTAuthentiation>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
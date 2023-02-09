using Microsoft.EntityFrameworkCore;
using NewsBook.Data;
using NewsBook.Repository;
using NewsBook.Authorization;
using System.Text.Json.Serialization;
using NewsBook.Helpers;
using NewsBook.IdentityServices;
using MediatR;

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
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer("server=.;database=newsdb;trusted_connection=true;Encrypt=False"));
            //builder.Services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("DbNews"));
            // configure strongly typed settings object
            builder.Services.Configure<AppSettingsDTO>(builder.Configuration.GetSection("appsettings"));
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<INewsRepository, NewsRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IFavouriteNewsRespository, FavouriteNewsRespository>();
            builder.Services.AddScoped<IJwtUtils, JWTService>();
            builder.Services.AddScoped<IIdentityServices, NewsBook.IdentityServices.IdentityServices>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMediatR(typeof(Program).Assembly);
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
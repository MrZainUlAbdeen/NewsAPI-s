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

namespace NewsBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("DbNews"));


            //var key = "Newsbook1234@54>sa";
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(o =>
            //{
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //        //ValidAudience = builder.Configuration["Jwt:Audience"],
            //        //IssuerSigningKey = new SymmetricSecurityKey
            //        //(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            //        //ValidateIssuer = true,
            //        //ValidateAudience = true,
            //        //ValidateLifetime = false,
            //        //ValidateIssuerSigningKey = true
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            //        ValidateIssuer = true,
            //        ValidateAudience = false
            //    };
            //});




            //JWT Token
            //var key = "Newsbook1234@54>sa";
            //builder.Services.AddAuthentication(x =>
            //x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme

            //).AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters

            //    {

            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            //        ValidateIssuer = true,
            //        ValidateAudience = false
            //    };
            //});
            //builder.Services.AddSingleton<JWTAuthentiation>(new JWTAuthentiation(key));
            //Close
            builder.Services.AddScoped<INewsRepository, NewsRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IFavouriteNewsRespository, FavouriteNewsRespository>();
            
            builder.Services.AddTransient<IJwtUtils, JWTAuthentiation>();
         
            //builder.Services.AddScoped<IJwtUtils, JWTAuthentiation>();
            builder.Services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/api/[controller]";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(1);
                });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
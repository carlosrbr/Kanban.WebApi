
namespace webapi_kanban
{
    using DotNetEnv;
    using FluentValidation;
    using Kanban.Application;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Repository;
    using Kanban.Domain.Interfaces.Service;
    using Kanban.Domain.Validators;
    using Kanban.Infrastructure.Data.Context;
    using Kanban.Infrastructure.Data.Repository;
    using Microsoft.AspNetCore.Authentication.JwtBearer;    
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using webapi_kanban.Middlewares;

    public partial class Program
    {
        public static void Main(string[] args)
        {
            var corsName = "kanbanCors";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<KanbanDbContext>(options => options.UseInMemoryDatabase("KanbanInMemoryDb"));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: corsName,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000")
                                          .AllowAnyHeader()
                                          .AllowAnyMethod();
                                  });
            });


            Env.Load();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ICardService, CardService>(); 
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<IValidator<Card>, CardValidator>();


            // Configurar serviços
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseMiddleware<LoggingMiddleware>();

            app.UseCors(corsName);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

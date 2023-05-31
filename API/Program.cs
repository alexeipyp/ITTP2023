using API.AutoMapper;
using API.Middleware;
using BLL.AutoMapper;
using BLL.Configs;
using BLL.Services;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var authSection = builder.Configuration.GetSection("Auth");
            var authConfig = authSection.Get<AuthConfig>();

            builder.Services.Configure<AuthConfig>(authSection);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "Insert Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme,
                            },
                            Scheme = "oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                c.SwaggerDoc("Auth", new OpenApiInfo { Title = "Auth" });
                c.SwaggerDoc("Api", new OpenApiInfo { Title = "Api" });
            });

            builder.Services.AddDbContext<DAL.DataContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"), sql => { });
            });

            builder.Services.AddAutoMapper(typeof(ModelsAndEntitiesMapperProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(HTTPDTOsToBLLModelsMapperProfile).Assembly);

            builder.Services.AddScoped<UserRepository>();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = authConfig.Issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = authConfig.SymmetricSecuriryKey(),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            builder.Services.AddAuthorization(o =>
            {
                o.AddPolicy("ValidAccessToken", p =>
                {
                    p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    p.RequireAuthenticatedUser();
                });
            });


            var app = builder.Build();

            using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                if (serviceScope != null)
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<DAL.DataContext>();
                    context.Database.Migrate();
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("Api/swagger.json", "Api");
                    c.SwaggerEndpoint("Auth/swagger.json", "Auth");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCustomExceptionHandler();
            app.UseTokenValidator();
            app.MapControllers();

            app.Run();
        }
    }
}
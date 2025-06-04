using MailSender.Application;
using MailSender.Application.Managers;
using MailSender.Application.Managers.Interfaces;
using MailSender.Application.Services;
using MailSender.Application.Services.Interfaces;
using MailSender.Contracts.Mappers;
using MailSender.Contracts.Settings;
using MailSender.Infrastructure.EmailSender;
using MailSender.Infrastructure.EmailSender.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Resend;
using System.Reflection;

namespace MailSender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerGen(option =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "MailSender REST API docs", Version = "in dev" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IMailSenderProvider, ResendMailSender>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();
            builder.Services.AddScoped<IMailManager, MailManager>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.Configure<BrevoSettings>(builder.Configuration.GetSection("BrevoSettings"));
            builder.Services.Configure<ResendSettings>(builder.Configuration.GetSection("ResendSettings"));

            builder.Services.AddOptions();
            builder.Services.AddHttpClient<ResendClient>();
            builder.Services.Configure<ResendClientOptions>(options =>
            {
                options.ApiToken = builder.Configuration["ResendSettings:ApiKey"];
            });
            builder.Services.AddTransient<IResend, ResendClient>();


            var signingKey = builder.Configuration["JWT:SigningKey"];
            if (string.IsNullOrWhiteSpace(signingKey))
            {
                throw new Exception("JWT:SigningKey is missing or empty in appsettings.json");
            }
            var issuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = issuerSigningKey,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

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

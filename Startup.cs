using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Interfaces.Repositories;
using WebAPI.Inputs;
using WebAPI.Logging;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Schema.Context;
using WebAPI.SimpleTokenProvider;
using WebAPI.SimpleTokenProvider.Model;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<HandyManContext>(options =>
                //options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // services.AddIdentity<ApplicationUser, IdentityRole>()
            //     .AddEntityFrameworkStores<HandyManContext>()
            //     .AddDefaultTokenProviders();

            // Automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<OrdersRecord, OrderDto>();
                cfg.CreateMap<OrderDto, OrdersRecord>();
            });

            // Dependency container
            services.AddScoped<IOrdersRepository, OrdersRepostiory>();

            // Add cors
            services.AddCors();

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        // The secret key every token will be signed with.
        // In production, you should store this securely in environment variables
        // or a key management tool. Don't hardcode this into your application!
        private static readonly string secretKey = "mysupersecret_secretkey!123";
        private static readonly string _authorityUrl = "http://localhost/api/";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, HandyManContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // //call ConfigureLogger in a centralized place in the code
            // ApplicationLogging.ConfigureLogger(loggerFactory);
            // //set it as the primary LoggerFactory to use everywhere
            // ApplicationLogging.LoggerFactory = loggerFactory;

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
                builder.WithOrigins(_authorityUrl).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

            // secretKey contains a secret passphrase only your server knows
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = _authorityUrl,
                IssuerSigningKey = signingCredentials.Key
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                Audience = _authorityUrl,
                AutomaticAuthenticate = true,
                RequireHttpsMetadata = false,
                TokenValidationParameters = tokenValidationParameters
            });

            // Add JWT generation endpoint:
            var options = new TokenProviderOptions
            {
                Audience = _authorityUrl,
                Issuer = _authorityUrl,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));

            app.UseMvc();
            DbInitializer.Initialize(context);
        }
    }
}

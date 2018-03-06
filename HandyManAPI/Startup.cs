using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using HandyManAPI.Helpers;
using HandyManAPI.Inputs;
using HandyManAPI.Interfaces.Repositories;
using HandyManAPI.Interfaces.Services;
using HandyManAPI.Models;
using HandyManAPI.Repositories;
using HandyManAPI.Schema.Context;
using HandyManAPI.Services.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HandyManAPI
{
    public class Startup
    {
        private static readonly string _authorityUrl = "http://localhost/api/";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HandyManContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<OrderRecord, OrderDto>()
                    .ForMember(x => x.ImgAttachments, opt => opt.Ignore());

                cfg.CreateMap<OrderDto, OrderRecord>()
                    .ForMember(x => x.ImgAttachments, opt => opt.Ignore()); ;

                cfg.CreateMap<UserRecord, UserDto>();
                cfg.CreateMap<UserDto, UserRecord>();
            });

            // Dependency container
            services.AddScoped<IOrderRepository, OrderRepostiory>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailService, EmailService>();

            // Add cors
            services.AddCors();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, HandyManContext context)
        {
            app.UseStaticFiles();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // // Shows UseCors with CorsPolicyBuilder.
            // app.UseCors(builder =>
            //     builder.WithOrigins(_authorityUrl).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials());
            app.UseAuthentication();
            app.UseMvc();

            DbInitializer.Initialize(context);
        }
    }
}


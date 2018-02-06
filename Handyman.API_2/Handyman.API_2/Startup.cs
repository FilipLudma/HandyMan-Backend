using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Handyman.API_2.Data;
using Handyman.API_2.Models;
using Handyman.API_2.Services;
using WebAPI.Models;
using WebAPI.Inputs;
using WebAPI.Interfaces.Repositories;
using WebAPI.Repositories;
using WebAPI.Schema.Context;

namespace Handyman.API_2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

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

            //services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // Add MVC
            services.AddMvc().AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }

            // app.Use(next => context =>
            // {
            //     if (string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase))
            //     {
            //         // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
            //         var tokens = antiforgery.GetAndStoreTokens(context);
            //         context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            //             new CookieOptions() { HttpOnly = false });
            //     }

            //     return next(context);
            // });

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
                builder.WithOrigins().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
            DbInitializer.Initialize(context);
        }
    }
}

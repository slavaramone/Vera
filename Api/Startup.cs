using Api.Security;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using Domain.Interfaces;
using Domain.Options;
using Domain.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Api
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions()
                .AddMvc()
                .AddJsonOptions(options => { options.SerializerSettings.Formatting = Formatting.Indented; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddCors();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.ConfigureAuthentication(Configuration);

            services.AddOptions()
                .Configure<AuthOptions>(Configuration.GetSection("Auth"))
                .Configure<SmscOptions>(Configuration.GetSection("Smsc"));

            services.AddAutoMapper(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }, Assembly.GetExecutingAssembly());

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<VeraDbContext>(options =>
                {
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    options.UseSqlServer(Configuration.GetConnectionString("Vera"));
                });

            var optionsBuilder = new DbContextOptionsBuilder<VeraDbContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Vera"));
            using (var _db = new VeraDbContext(optionsBuilder.Options))
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }                
            }

            services.AddSingleton<ISmsSender, SmsDeliveryService>();
            services.AddSingleton<JwtFactory>();
            services.AddScoped<IVeraDbRepository, VeraDbRepository>();
            services.AddScoped<IClaimsAccessor, ClaimsAccessor>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Vera REST API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
			app.UseOptions();

			if (env.IsEnvironment("Dev") || env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddFile($"C:/Logs/Vera-{env.EnvironmentName}.log");

			app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

			app.UseAuthentication();

			app.UseExceptionHandler(applicationBuilder =>
            {
                applicationBuilder.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null)
                    {
                        await context.Response.WriteAsync(new
                        {
                            context.Response.StatusCode,
                            Message = $"Internal Server Error. TraceIdentifier:{context.TraceIdentifier}, {feature.Error.Message}, {feature.Error.StackTrace}",
                        }.ToString());
                    }
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vera REST API v1");
                c.RoutePrefix = String.Empty;
            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();

            lifetime.ApplicationStopped.Register(LogManager.Shutdown);
        }
    }
}

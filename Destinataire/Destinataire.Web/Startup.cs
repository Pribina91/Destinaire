using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Destinataire.Core.Interfaces;
using Destinataire.Core.Repositories;
using Destinataire.Core.Services;
using Destinataire.Data;
using Destinataire.Web.Dtos;
using Destinataire.Web.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Destinataire.Web
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
            services.AddDbContext<DestinaireContext>(options => options.UseSqlite(Configuration.GetConnectionString("sqlConnection")));
            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<HealthChecksFilter>();
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Destinaire API", Version = "v1",
                        Contact = new OpenApiContact
                        {
                            Name = "Andrej Stajer",
                            Email = "astajer@pribitech.com",
                            Url = new Uri("http://pribitech.com/"),
                        },
                    });
            });

            services.AddHealthChecks();

            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddAutoMapper(typeof(Startup));
            //services.AddControllersWithViews();
            //todo add
            //services.AddResponseCompression(options =>
            //{
            //    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            //        new[] { MediaTypeNames.Application.Octet });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Destinaire API");
            });

            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var ex = error.Error;

                        await context.Response
                            .WriteAsync(JsonConvert.SerializeObject(new ErrorDto()
                            {
                                StatusCode = 500,
                                ErrorMessage = ex.Message
                            }));
                    }
                });
            }); 
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks(HealthChecksFilter.HealthCheckEndpoint, new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                });
                endpoints.MapControllers();
            });

        }
    }
}
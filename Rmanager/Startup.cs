    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Rmanager.Models;
using Rmanager.Dto;
using Rmanager.Exceptions;
using Rmanager.Services;
using AutoMapper;

namespace Rmanager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact{};
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "No lisence, all rights reserved",
                        Url = string.Empty
                    };
                };
            });
            services.AddControllers();
            //appsettings.jsonµÄ×¢²á
            IConfiguration config;
            //_key="DatabaseSettings"
            config = Configuration.GetSection(nameof(DatabaseSettings));
            services.Configure<DatabaseSettings>(config);

            services.AddAutoMapper(config =>
            {
                config.CreateMap<UserDetailDto, RmanagerUser>();
                config.CreateMap<UserEditInfoDto, RmanagerUser>();
                config.CreateMap<LogInDto, RmanagerUser>();
                config.CreateMap<Room, RoomDetailDto>();
                config.CreateMap<Booking, BookingRoomDto>();
                config.CreateMap<Organization, OrganizationDetailDto>();
                config.CreateMap<RoomBookingRecord, BookingDetailsDto>();
            }, typeof(UserDetailDto), typeof(RmanagerUser), typeof(UserEditInfoDto)
            , typeof(LogInDto),typeof(Room),typeof(RoomDetailDto), typeof(Booking)
            ,typeof(Organization),typeof(OrganizationDetailDto),typeof(RoomBookingRecord)
            ,typeof(BookingDetailsDto)
            );

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddSingleton<RmanagerService>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(op =>
                {
                    op.Events.OnRedirectToAccessDenied += (o) => throw new _403Exception("UnAuthorized!");
                });
        }
        
        



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseOpenApi(config =>
            {
                config.PostProcess = (doc, rec) =>
                {
                    doc.Schemes.Clear();
                    doc.Schemes.Add(NSwag.OpenApiSchema.Https);
                    rec.Scheme = "https";
                };
            });
            app.UseSwaggerUi3(config =>
            {
                
            });
            app.UseReDoc(conf =>
            {
                conf.Path = "/api/docs";
            });
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "User", pattern: @"{url}",
                    defaults: new { controller = "User",action= "Get" }
                    );
            });
        }
    }
}

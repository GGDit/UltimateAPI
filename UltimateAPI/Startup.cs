using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Extensions;
using Contracts;
using ActionFilters.Filters;
using Entities.DataTransferObjects;
using Repository;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); //�������� ���������� ������������ �� ����� nlog.config

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();

            services.AddControllers();

            services.ConfigureSqlContext(Configuration);

            services.ConfigureRepositoryManager();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers(config => 
            { 
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
              .AddXmlDataContractSerializerFormatters()
              .AddCustomCSVFormatter();

            services.Configure<ApiBehaviorOptions>(options => 
            { 
                options.SuppressModelStateInvalidFilter = true; 
            });

            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateCompanyExistsAttribute>();
            services.AddScoped<ValidateEmployeeForCompanyExistsAttribute>();

            services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

            services.ConfigureVersioning();

            services.AddAuthentication(); 
            services.ConfigureIdentity();
            //services.ConfigureJWT(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();

            app.UseStaticFiles(); //��������� ������������ ����������� ����� ��� �������. ���� �� �� ������ ���� � �������� ����������� ������, �� ��������� �� ����� ������������ ����� wwwroot � ����� �������.
            app.UseCors("CorsPolicy"); //��������� �������� CORS
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All }); //������������ ��������� ������ � ������� ������. ��� ������� ��� ��� ������������� ����������.

            app.UseRouting(); //��������� ������� �������������

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization(); //��������� ������� �����������

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }); //��������� �������� ����� ��� �������� ����������� � ������������� ��� �������� �����-���� ���������          
        }
    }
}

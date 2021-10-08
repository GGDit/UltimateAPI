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
using UltimateAPI.Extensions;

namespace UltimateAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); //загрузка параметров логгирования из файла nlog.config

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(); //Позволяет использовать статические файлы для запроса. Если мы не укажем путь к каталогу статических файлов, по умолчанию он будет использовать папку wwwroot в нашем проекте.
            app.UseCors("CorsPolicy"); //Добавляет политики CORS
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All }); //перенаправит заголовки прокси в текущий запрос. Это поможет нам при развертывании приложения.

            app.UseRouting(); //Добавляют функции маршрутизации

            app.UseAuthorization(); //Добавляют функции авторизации

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }); //Добавляет конечную точку для действия контроллера в маршрутизацию без указания каких-либо маршрутов
        }
    }
}

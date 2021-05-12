using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using VacationsAPI.Models.Department;
using VacationsAPI.Models.User;
using VacationsAPI.Models.Vacation;
using VacationsAPI.Models.Worker;

namespace VacationsAPI
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
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            services.AddControllers();
            ConfigureDatabase(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VacationsAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationsAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.Configure<DepartmentDatabaseSettings>(
                Configuration.GetSection(nameof(DepartmentDatabaseSettings)));
            services.AddSingleton<IDepartmentDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DepartmentDatabaseSettings>>().Value);
            //Подключение MongoDepartmentRepository
            services.AddSingleton<MongoDepartmentRepository>();

            services.Configure<WorkerDatabaseSettings>(
                Configuration.GetSection(nameof(WorkerDatabaseSettings)));
            services.AddSingleton<IWorkerDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<WorkerDatabaseSettings>>().Value);
            //Подключение MongoWorkerRepository
            services.AddSingleton<MongoWorkerRepository>();

            services.Configure<VacationDatabaseSettings>(
                Configuration.GetSection(nameof(VacationDatabaseSettings)));
            services.AddSingleton<IVacationDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<VacationDatabaseSettings>>().Value);
            //Подключение MongoVacationRepository
            services.AddSingleton<MongoVacationRepository>();
            
            services.Configure<UserDatabaseSettings>(
                Configuration.GetSection(nameof(UserDatabaseSettings)));
            services.AddSingleton<IUserDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<UserDatabaseSettings>>().Value);
            //Подключение MongoVacationRepository
            services.AddSingleton<MongoUserRepository>();
        }
    }
}

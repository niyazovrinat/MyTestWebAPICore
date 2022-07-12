using System;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TodoApi_Business.Services;
using TodoApi_Business.Services.IServices;
using TodoApiDTO_Business.Mapping;
using TodoApiDTO_DataAccess.Data;
using TodoApiDTO_DataAccess.Repository;
using TodoApiDTO_DataAccess.Repository.IRepository;
using TodoApiDTO_Models.DTO;

namespace TodoApi
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
            //Подключаем базу данных
            services.AddDbContext<TodoContext>(opt =>
               opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Подключаем AutoMapper
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers()
                .AddFluentValidation(x => //Задаем настройки для FluentValidation
                {
                    x.DisableDataAnnotationsValidation = true;
                    x.RegisterValidatorsFromAssemblyContaining<TodoItemDTOValidator>();
                });

            //Подключаем репозитории и сервисы к DI
            services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();
            services.AddScoped<ITodoItemsService, TodoItemsService>();

            //Подключить Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TodoApiDTO",
                    Description = "Тестовый API для работы с задачами",
                    TermsOfService = new Uri("https://TodoApiDTO.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Niyazov Rinat",
                        Email = "testtest@gmail.com",
                        Url = new Uri("https://TodoApiDTO.com/contacts")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Указываем настройки Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApiDTO");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

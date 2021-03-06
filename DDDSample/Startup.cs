﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Akka.Actor;
using DDDSample.Adapters.kafka;
using DDDSample.Domain;
using DDDSample.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;


namespace DDDSample
{
    public class Startup
    {
        private readonly KafkaConsumer consumer;

        //TODO : 설정화
        private string kafkaServer = "kafka:9092";
        private string kafkaTopic = "test_consumer";


        public Startup(IConfiguration configuration)
        {
            consumer = new KafkaConsumer(kafkaServer, kafkaTopic);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<ActorSystem>(_ => ActorSystem.Create("cqrsakka"));

            services.AddSingleton<KafkaProduce>(_ => new KafkaProduce(kafkaServer, kafkaTopic));

            services.AddDbContext<UserRepository>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });            

        }

        protected void initDomain(IApplicationBuilder app,IServiceScope serviceScope, IHostingEnvironment env)
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<UserRepository>();
            if (env.IsDevelopment())
            {
                //Test for Clean
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
            }

            var actorSystem = serviceScope.ServiceProvider.GetRequiredService<ActorSystem>();
            System.Console.WriteLine("Actor System Check===" + actorSystem.Name);

            var userInsertActor = actorSystem.ActorOf(InsertUser.Props(context), "userInsertActor1");

            consumer.CreateConsumer(userInsertActor).Start();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            initDomain(app, serviceScope, env);

            if (env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
            
        }

        private void OnShutdown()
        {            
            consumer.Stop();
            System.Threading.Thread.Sleep(3000);
        }

    }
}

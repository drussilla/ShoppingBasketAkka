using Akka.Actor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingBasket.Domains.BasketDomain;
using ShoppingBasket.Domains.ProductDomain;
using Swashbuckle.AspNetCore.Swagger;

namespace ShoppingBasket
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Shopping Basket API", Version = "v1" });

            });

            // we do not need to expose whole ActorSystem class to the system,
            // IActorRefFactory is enough for the functionality we have.
            services.AddSingleton<IActorRefFactory>(_ => ActorSystem.Create("shopping-basket"));

            // IF amount of registration grows we can move them to separate extension methods
            services.AddSingleton<IProductsDataSource, ProductsDataSource>();
            services.AddSingleton<ProductActorProvider>();

            services.AddSingleton<BasketManagerActorProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Basket API V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

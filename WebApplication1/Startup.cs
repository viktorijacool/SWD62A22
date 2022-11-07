using BusinessLogic.Services;
using DataAccess.Context;
using DataAccess.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
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
            services.AddDbContext<ShoppingCartContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ShoppingCartContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();


            //in this Inversions Of Control Class, we are going to let it know what instances may be asked at a later stage to be created there
            //teh Startup.cs (this class) needs to know what life expectancy the service class needs to be created with

            //Transient
            //Scoped
            //Singleton

            /*
             * 
             * Singlenton: IoC container will create and share a single instance of a servie throughout the application's lifetime.
             * e.g. if there are 500 users browsing your website, only ONE instance of ItemService will be created and shared among all those users
             * e.g. usage Chat Server may use Singlenton instance to host all the chat rooms
             * 
             * 
             * Transient: The IoC container will create a new instance of the specified service type every time you ask for it.
             * e.g. if there are 50 users and there is a request for ItemsService, then 50 instances will be created 
             * e.g. if there are 50 users and there is 2 requests for ItemsRepository (within the ItemsService), then 50x2=100 instances of ItemsRepository in memory ...
             * 
             * 
             * Scoped: IoC container will create an instance of the specified service type once per request and will be shared in a single request.
             * e.g. if there are 50 users and there are 2 calls for ItemsRepository (within the ItemsService), then 50x1=50 instances of ItemsRepository in memory ...
             * 
             * 
             * 
             */

            services.AddScoped<ItemsServices>();
            services.AddScoped<ItemsRepository>();

            //We are instructing the crl so that when it comes across ICategoriesRepository (in the constructor), it should initialize the class daclared after the comma
            
            FileInfo fi = new FileInfo(@"D:\MCAST\Enterprise Proramming\EnterpriseProgrammingSolution\WebApplication1\Data\categories.txt");
            //reads categories from a file
            //services.AddScoped<ICategoriesRepository, CategoriesFileRepository>(x => new CategoriesFileRepository(fi));
            
            //reads categories from a db
            services.AddScoped<ICategoriesRepository, CategoriesFileRepository>();

            services.AddScoped<CategoriesServices>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

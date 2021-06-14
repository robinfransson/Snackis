using SnackisDB.Models;
using SnackisDB.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SnackisForum.Injects;

namespace SnackisForum
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IServiceProvider ServiceProvider { get; }
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if(CurrentEnvironment.IsDevelopment())
            {
                Console.WriteLine("Vilken anslutningssträng ska användas? 'local' eller 'azure'");
                string connectionString = Console.ReadLine();
                services.AddDbContext<SnackisContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString(connectionString));
                });
            }
            else
            {   
                services.AddDbContext<SnackisContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("azure"));
                });
            }

            services.AddAuthorization();
            services.AddRazorPages();
            services.AddHttpContextAccessor();

            services.AddScoped<SnackisForum.Injects.UserProfile>();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");





            services.AddHttpClient();

            services.AddIdentity<SnackisUser, IdentityRole>(options =>
            {

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
               
            })
                .AddEntityFrameworkStores<SnackisContext>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders();


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Index";
                options.LoginPath = "/Index";
            });

            services.AddScoped<SetupDb>();





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, SetupDb setup)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseLiveReload();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            setup.Setup();
        }
    }
}

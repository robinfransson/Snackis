using SnackisDB.Models;
using SnackisDB.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Westwind.AspNetCore.LiveReload;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace SnackisForum
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if(CurrentEnvironment.IsDevelopment())
            {
                services.AddAuthorization(options =>
                {
                    //options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                });
                services.AddRazorPages(options =>
                {
                    //options.Conventions.AuthorizeFolder("/Admin", "RequireAdminRole");
                }
                ).AddRazorRuntimeCompilation();
                //services.AddLiveReload();
                Console.WriteLine("Vilken sträng ska användas?");
                string connectionString = Console.ReadLine();
                services.AddDbContext<SnackisContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString(connectionString));
                });
            }
            else
            {
                services.AddAuthorization(options =>
                {
                    //options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                });
                services.AddRazorPages(options =>
                {
                    //options.Conventions.AuthorizeFolder("/Admin", "RequireAdminRole");
                });
                
                services.AddDbContext<SnackisContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("azure"));
                });
            }
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


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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
        }
    }
}

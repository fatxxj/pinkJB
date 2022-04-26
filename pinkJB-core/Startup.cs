
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pinkJB_core.Data;
using pinkJB_core.Services;
using pinkJB_core.Data.Cart;
using System;
using System.Globalization;
using DinkToPdf.Contracts;
using DinkToPdf;
using pinkJB_core.Models;
using Microsoft.AspNetCore.Localization;

namespace pinkJB_core
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
            //DB context config
            services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));
            services.AddSession();
            services.AddControllersWithViews();

            services.AddScoped<IProductsService, ProductService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            //auth - aur
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme=CookieAuthenticationDefaults.AuthenticationScheme;

            });
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));



            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });
            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

           



            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();


        }

        public virtual string ISOCurrencySymbol { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRequestLocalization();
        //    app.UseRequestLocalizationCookies();

            app.UseRouting();
            app.UseSession();
            //auth and aur
            app.UseAuthentication();
            app.UseAuthorization();


            var supportedCultures = new[] { "en", "sq", "mk" };
            var localisationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0]).AddSupportedCultures(supportedCultures).AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localisationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
              //Seed database
            AppDbInitializer.Seed(app);
            //AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();
        }

      
    }
}

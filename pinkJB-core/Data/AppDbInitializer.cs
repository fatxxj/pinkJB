using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using pinkJB_core.Data.Static;
using pinkJB_core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pinkJB_core.Data
{
    public class AppDbInitializer
    {

        public static void Seed(IApplicationBuilder applicaitonBuilder)
        {

            using (var serviceScope = applicaitonBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                //Products
                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
                       {
                           new Product
                       {
                           ProductName = "Fancy pyjama",
                           ProductDescription = "Pyjamas loungewar chill Morena Taraku",
                           ProductPrice = 7.99,
                          // ProductCategory = Enums.ProductCategory.Pyjama,
                           ProductImage = "https://dotnethow.net/images/actors/actor-4.jpeg",
                           amountLeft = 100
                       },

                       new Product
                       {
                           ProductName = "Loungewear",
                           ProductDescription = " loungewar chill Ariola BB",
                           ProductPrice = 7.99,
                           //ProductCategory = Enums.ProductCategory.LoungeWear,
                           ProductImage = "https://dotnethow.net/images/actors/actor-2.jpeg",
                           amountLeft=50
                       },

                       new Product
                       {
                           ProductName = "Ne Kese njo",
                           ProductDescription = "Nightwear perputhen",
                           ProductPrice = 7.99,
                          // ProductCategory = Enums.ProductCategory.NightWear,
                           ProductImage = "https://dotnethow.net/images/actors/actor-2.jpeg",
                           amountLeft=0

                       },



                       });
                    context.SaveChanges();
                }
            }

        }
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@pinkJb.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if(adminUser==null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin user",
                        UserName = "Admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true


                    };
                    await userManager.CreateAsync(newAdminUser, "BufiBufi1234");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);

                }


                string appUserEmail = "user@pinkJb.com";
                var appUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application user",
                        UserName = "bufiUser",
                        Email = appUserEmail,
                        EmailConfirmed = true


                    };
                    await userManager.CreateAsync(newAppUser, "BufiBufi1234");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);

                }
            }
        }



    }

}



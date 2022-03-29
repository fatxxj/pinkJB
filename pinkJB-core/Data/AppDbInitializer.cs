using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using pinkJB_core.Models;
using System.Collections.Generic;
using System.Linq;

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
    }
}



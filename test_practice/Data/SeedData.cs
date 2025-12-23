using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using test_practice.Models;
using Microsoft.EntityFrameworkCore;
namespace test_practice.Data

{
    public class SeedData
    {
        public static async Task Initialize(
            ApplicationDbContext context)
        {

            await context.Database.EnsureCreatedAsync();

        

            if (!context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Name = "Смартфоны" },
                    new Category { Name = "Ноутбуки" },
                    new Category { Name = "Наушники" },
                    new Category { Name = "Аксессуары" },
                };

                await context.Categories.AddRangeAsync(categories);

                await context.SaveChangesAsync();
            }
           if (!context.Products.Any())
           {
                var category1 = await context.Categories.FirstAsync(c => c.Name == "Смартфоны");
                var category2 = await context.Categories.FirstAsync(c => c.Name == "Ноутбуки");
                var category3 = await context.Categories.FirstAsync(c => c.Name == "Наушники");

                var products = new[]
                {
                    new Product
                    {
                        Name = "Iphone 17",
                        Description = "bebebebebe",
                        Price = 200000,
                        ImageUrl = "https://placehold.co/300x200",
                        CategoryId = category1.Id,
                        StockQuantity = 25
                    },
                    new Product 
                    { 
                        Name = "Samsung Galaxy S24 Ultra",
                        Description = "GOAT",
                        Price = 150000,
                        ImageUrl = "https://placehold.co/300x200",
                        CategoryId = category1.Id,
                        StockQuantity = 25
                
                    },
                    new Product
                    {
                        Name = "Mac Book Pro",
                        Description = "default mac",
                        Price = 200000,
                        ImageUrl = "https://placehold.co/300x200",
                        CategoryId = category2.Id,
                        StockQuantity = 25
                    },
                    new Product
                    {
                        Name = "Dell Ultra",
                        Description = "best for random guy",
                        Price = 50000,
                        ImageUrl = "https://placehold.co/300x200",
                        CategoryId = category2.Id,
                        StockQuantity = 25
                    },
                    new Product
                    {
                        Name = "Air Pods Pro",
                        Description = "air pods",
                        Price = 10000,
                        ImageUrl = "https://placehold.co/300x200",
                        CategoryId = category3.Id,
                        StockQuantity = 25
                    },
                    new Product
                    {
                        Name = "Redmi Buds",
                        Description = "smth",
                        Price = 5000,
                        ImageUrl = "https://placehold.co/300x200",
                        CategoryId = category3.Id,
                        StockQuantity = 25
                    },
                };
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();

           }
        }


    }
}

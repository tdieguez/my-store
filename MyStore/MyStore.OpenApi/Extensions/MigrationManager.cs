using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyStore.OpenApi.Data;
using MyStore.OpenApi.Entities;

namespace MyStore.OpenApi.Extensions
{
    public static class MigrationExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MyStoreDbContext>();

            context.Database.Migrate();

            if (!context.Categories.Any())
                context.Categories.AddRange(InitialCategories);

            if (!context.Products.Any())
                context.Products.AddRange(InitialProducts(InitialCategories.First()));

            context.SaveChanges();

            return host;
        }

        private static ICollection<Category> InitialCategories = new List<Category>
        {
            new Category
            {
                Name = "Food",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras rhoncus, lectus vel efficitur blandit.",
                CreatedAt = DateTimeOffset.Now.AddDays(-150),
                ModifiedAt = DateTimeOffset.Now.AddDays(-109)
            },
            new Category
            {
                Name = "Drink",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean porttitor mattis dolor sit amet nec.",
                CreatedAt = DateTimeOffset.Now.AddDays(-140),
                ModifiedAt = DateTimeOffset.Now.AddDays(-115)
            }
        };

        private static ICollection<Product> InitialProducts(Category category) => new List<Product>
        {
            new()
            {
                Name = "Moqueca Capixaba",
                Description =
                    "Brazilian seafood stew. It is slowly cooked in a terracotta cassole. Moqueca can be made with shrimp or fish as a base with tomatoes, onions, garlic, lime and coriander.",
                Category = category,
                Price = 99.99,
                CreatedAt = DateTimeOffset.Now.AddDays(-100),
                ModifiedAt = DateTimeOffset.Now.AddDays(-99)
            },
            new()
            {
                Name = "Torta Capixaba",
                Description =
                    "Traditional and complex Brazilian dish originated from Espirito Santo. This seafood pie is made with a massive list of ingredients: fish such as sea bass, hake, and grouper, mussels, siri crabmeat, oysters, salt cod, shrimp, olive oil, garlic, onions, tomatoes, green onions, cilantro, red bell peppers, annatto oil, coconut milk, cloves, cinnamon, white vinegar, palm hearts, olives, and eggs.",
                Category = category,
                Price = 66.66,
                CreatedAt = DateTimeOffset.Now.AddDays(-70),
                ModifiedAt = DateTimeOffset.Now.AddDays(-64)
            }
        };
    }
}
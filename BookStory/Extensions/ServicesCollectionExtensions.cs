using BookStory.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStory.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static void AddMainContext(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var inMemory = configuration.GetValue<bool>("InMemory");
            if (inMemory)
            {
                serviceCollection.AddDbContext<MainContext>(builder =>
                {
                    builder.UseInMemoryDatabase("Main");
                });
            }
            else
            {
                serviceCollection.AddDbContext<MainContext>(builder =>
                {
                    builder.UseNpgsql(configuration.GetConnectionString("Default"));
                });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using BookStory.Data.Contexts;
using BookStory.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStory.Data.Seeders
{
    public class MainContextSeeder
    {
        public void Seed(
            MainContext context,
            ILogger logger)
        {
            try
            {
                if (context.Database.IsRelational() && context.Database.GetPendingMigrations().Any())
                {
                    throw new ArgumentException($"Имеются не примененные миграции для контекста {nameof(MainContext)}." +
                                                " Пожалуйста примените их `dotnet ef database update`");
                }

                if (!context.Genres.Any())
                {
                    logger.LogInformation("Пытаемся \"посадить\" записи жанров в БД");
                    context.Genres.AddRange(GetPreconfiguredGenres());

                    context.SaveChanges();

                    logger.LogInformation("Записи о жанрах успешно \"посажены\" в БД");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }

        private static IEnumerable<Genre> GetPreconfiguredGenres() => new[]
        {
            new Genre {Id = 1, Name = "Фантастика"},
            new Genre {Id = 2, Name = "Роман"},
            new Genre {Id = 3, Name = "Приключения"}
        };
    }
}
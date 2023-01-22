using System;
using Microsoft.EntityFrameworkCore;

namespace BookStory.Helpers
{
    public static class ErrorHelper
    {
        public static Exception ContextIsNotRegistered<TContext>()
            where TContext : DbContext
        {
            return new ArgumentException($"{typeof(TContext).FullName} не зарегистрирован в DI контейнере");
        }
    }
}
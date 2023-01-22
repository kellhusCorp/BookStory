using System.Threading.Tasks;
using AutoMapper;
using BookStory.Data.Contexts;
using BookStory.Domain;
using BookStory.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookStory.Controllers.Base
{
    public class BaseMainController : ControllerBase
    {
        protected readonly MainContext context;

        protected readonly IMapper mapper;

        public BaseMainController(
            MainContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        protected virtual ObjectResult Created(object? value)
        {
            return new ObjectResult(value)
            {
                StatusCode = 201
            };
        }

        protected async Task<ActionResult> AddAuthorShip(int bookId, int authorId)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book == null) return NotFound("Не найдена книга");

            var author = await context.Authors.FindAsync(authorId);
            if (author == null) return NotFound("Не найден автор");

            if (await AuthorShipExists(bookId, authorId)) return BadRequest("Связь уже существует");

            var authorShip = new AuthorShip(bookId, authorId);

            await context.AuthorShips.AddAsync(authorShip);

            await context.SaveChangesAsync();

            return Created(mapper.Map<AuthorShipDto>(authorShip));
        }

        private async Task<bool> AuthorShipExists(int bookId, int authorId)
        {
            return await context.AuthorShips
                .FindAsync(bookId, authorId) != null;
        }

        protected async Task<ActionResult> RemoveAuthorShip(int bookId, int authorId)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book == null) return NotFound("Не найдена книга");

            var author = await context.Authors.FindAsync(authorId);
            if (author == null) return NotFound("Не найден автор");

            var authorShip = await context.AuthorShips.FindAsync(bookId, authorId);

            if (authorShip == null) return NotFound("Не найдена связь");

            context.AuthorShips.Remove(authorShip);

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
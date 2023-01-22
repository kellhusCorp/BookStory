using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStory.Controllers.Base;
using BookStory.Data.Contexts;
using BookStory.Domain;
using BookStory.Domain.Details;
using BookStory.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStory.Controllers
{
    [ApiController]
    [Route("/api/authors")]
    public class AuthorController : BaseMainController
    {
        public AuthorController(
            MainContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation("Возвращает список всех авторов")]
        public async Task<ActionResult<IEnumerable<AuthorWithBooksDto>>> Get()
        {
            var authors = await GetAuthorsWithBooks()
                .ToListAsync();

            return Ok(authors);
        }

        [HttpGet]
        [Route("{authorId:int}")]
        [SwaggerOperation("Возвращает автора")]
        public async Task<ActionResult<AuthorWithBooksDto>> GetById(int authorId)
        {
            var author = await InternalGetAuthorById(authorId);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("Создает автора")]
        public async Task<ActionResult<AuthorDto>> Create(AuthorDto request)
        {
            if (ModelState.IsValid)
            {
                var author = mapper.Map<Author>(request);

                await context.Authors.AddAsync(author);
                await context.SaveChangesAsync();

                return Created(await InternalGetAuthorById(author.Id));
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("{authorId:int}")]
        [SwaggerOperation("Обновляет информацию об авторе")]
        public async Task<ActionResult<AuthorWithBooksDto?>> Update(
            int authorId,
            [FromBody] AuthorDto request)
        {
            var author = await context.Authors.FindAsync(authorId);
            if (author == null)
            {
                return NotFound("Автор не найден");
            }

            if (ModelState.IsValid)
            {
                author.UpdateDetails(new AuthorDetails(request.Name, request.BirthDate));
                context.Authors.Update(author);
                await context.SaveChangesAsync();

                return Ok(await InternalGetAuthorById(author.Id));
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("{authorId:int}")]
        [SwaggerOperation("Удаляет автора")]
        public async Task<ActionResult> Delete(int authorId)
        {
            var author = await context.Authors.FindAsync(authorId);
            if (author == null)
            {
                return NotFound("Не найден автор");
            }

            context.Authors.Remove(author);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("{authorId:int}/books/{bookId:int}")]
        [SwaggerOperation("Добавить книгу к списку книг автора")]
        public async Task<ActionResult> AddBookToAuthor(int authorId, int bookId)
        {
            return await AddAuthorShip(bookId, authorId);
        }

        [HttpDelete]
        [Route("{authorId:int}/books/{bookId:int}")]
        [SwaggerOperation("Открепить книгу от списка книг автора")]
        public async Task<ActionResult> DeleteBookFromAuthor(int authorId, int bookId)
        {
            return await RemoveAuthorShip(bookId, authorId);
        }

        [HttpGet]
        [Route("{authorId:int}/books")]
        [SwaggerOperation("Возвращает список книг автора")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(int authorId)
        {
            var author = await GetAuthorsWithBooks()
                .FirstOrDefaultAsync(x => x.Id == authorId);

            if (author == null)
            {
                return NotFound("Не найден автор");
            }

            return Ok(author.Books);
        }

        private IQueryable<AuthorWithBooksDto> GetAuthorsWithBooks()
        {
            var authors = context.Authors
                .Include(x => x.AuthorShips)
                .ThenInclude(y => y.Book)
                .Select(x => new AuthorWithBooksDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    Books = x.AuthorShips.Select(z => new BookDto
                    {
                        Id = z.Book.Id,
                        EditorialOfficeName = z.Book.EditorialOfficeName,
                        GenreId = z.Book.GenreId,
                        Name = z.Book.Name,
                        PublishYear = z.Book.PublishYear
                    })
                })
                .AsNoTracking();

            return authors;
        }

        private async Task<AuthorWithBooksDto?> InternalGetAuthorById(int authorId)
        {
            var author = await GetAuthorsWithBooks()
                .FirstOrDefaultAsync(x => x.Id == authorId);

            return author;
        }
    }
}
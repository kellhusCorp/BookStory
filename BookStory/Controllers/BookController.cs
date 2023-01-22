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
    [Route("/api/books")]
    public class BookController : BaseMainController
    {
        public BookController(
            MainContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation("Возвращает все книги")]
        public async Task<ActionResult<IEnumerable<BookWithGenreAndAuthorsDto>>> Get()
        {
            return Ok(await GetBooksWithGenreAndAuthors()
                .ToListAsync());
        }

        [HttpGet]
        [Route("{bookId:int}")]
        [SwaggerOperation("Возвращает книгу")]
        public async Task<ActionResult<BookWithGenreAndAuthorsDto>> GetById(int bookId)
        {
            var book = await InternalGetBookById(bookId);

            if (book == null)
            {
                return NotFound("Не найдена книга");
            }

            return Ok(book);
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation("Создает книгу")]
        public async Task<ActionResult<BookWithGenreAndAuthorsDto>> Create(BookDto bookDto)
        {
            if (ModelState.IsValid)
            {
                if (!await GenreExists(bookDto.GenreId))
                {
                    return NotFound("Не найден жанр");
                }

                var book = mapper.Map<Book>(bookDto);

                await context.Books.AddAsync(book);

                await context.SaveChangesAsync();

                return Created(await InternalGetBookById(book.Id));
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("{bookId:int}")]
        [SwaggerOperation("Обновляет информацию о книге")]
        public async Task<ActionResult<BookWithGenreAndAuthorsDto>> Update(
            int bookId,
            [FromBody] BookDto request)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound("Книга не найдена");
            }

            if (!await GenreExists(request.GenreId))
            {
                return NotFound("Не найден жанр");
            }

            if (ModelState.IsValid)
            {
                book.UpdateDetails(new BookDetails(request.Name, request.PublishYear, request.GenreId, request.EditorialOfficeName));
                context.Books.Update(book);
                await context.SaveChangesAsync();

                return Ok(await InternalGetBookById(book.Id));
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("{bookId:int}")]
        [SwaggerOperation("Удаляет книгу")]
        public async Task<ActionResult> Delete(int bookId)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound("Не найдена книга");
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("{bookId:int}/authors/{authorId:int}")]
        [SwaggerOperation("Прикрепить автора к книге")]
        public async Task<ActionResult> AddAuthorToBook(int bookId, int authorId)
        {
            return await AddAuthorShip(bookId, authorId);
        }

        //TODO Можно добавить метод, для добавления авторов пачкой к книге
        // /api/{bookId}/authors/{ids}

        [HttpDelete]
        [Route("{bookId:int}/authors/{authorId:int}")]
        [SwaggerOperation("Открепить автора от книги")]
        public async Task<ActionResult> DeleteAuthorToBook(int bookId, int authorId)
        {
            return await RemoveAuthorShip(bookId, authorId);
        }

        [HttpGet]
        [Route("{bookId:int}/authors")]
        [SwaggerOperation("Возвращает список авторов книги")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(int bookId)
        {
            var book = await GetBooksWithGenreAndAuthors()
                .FirstOrDefaultAsync(x => x.Id == bookId);

            if (book == null) return NotFound("Книга не найдена");

            return Ok(book.Authors);
        }

        private IQueryable<BookWithGenreAndAuthorsDto> GetBooksWithGenreAndAuthors()
        {
            return context.Books
                .Include(x => x.Genre)
                .Include(y => y.AuthorShips)
                .ThenInclude(z => z.Author)
                .Select(x => new BookWithGenreAndAuthorsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PublishYear = x.PublishYear,
                    Genre = new GenreDto
                    {
                        Id = x.Genre.Id,
                        Name = x.Genre.Name
                    },
                    EditorialOfficeName = x.EditorialOfficeName,
                    Authors = x.AuthorShips.Select(a => new AuthorDto
                    {
                        Id = a.Author.Id,
                        Name = a.Author.Name,
                        BirthDate = a.Author.BirthDate
                    })
                })
                .AsNoTracking();
        }

        private async Task<BookWithGenreAndAuthorsDto?> InternalGetBookById(int bookId)
        {
            var book = await GetBooksWithGenreAndAuthors()
                .FirstOrDefaultAsync(x => x.Id == bookId);
            return book;
        }

        private async Task<bool> GenreExists(int genreId)
        {
            return await context.Genres.FindAsync(genreId) != null;
        }
    }
}
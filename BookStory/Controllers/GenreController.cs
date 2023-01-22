using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStory.Controllers.Base;
using BookStory.Data.Contexts;
using BookStory.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStory.Controllers
{
    [ApiController]
    [Route("/api/genres")]
    public class GenreController : BaseMainController
    {
        public GenreController(
            MainContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation("Возвращает все жанры")]
        public async Task<ActionResult<IEnumerable<GenreDto>>> Get()
        {
            var genres = await context.Genres.ToListAsync();
            return Ok(mapper.Map<IEnumerable<GenreDto>>(genres));
        }
    }
}
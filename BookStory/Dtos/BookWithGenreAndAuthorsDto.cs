using System.Collections.Generic;
using BookStory.Dtos.Base;

namespace BookStory.Dtos
{
    public class BookWithGenreAndAuthorsDto : BaseDto
    {
        public string Name { get; set; }

        public int PublishYear { get; set; }

        public GenreDto Genre { get; set; }

        public string EditorialOfficeName { get; set; }

        public IEnumerable<AuthorDto> Authors { get; set; }
    }
}


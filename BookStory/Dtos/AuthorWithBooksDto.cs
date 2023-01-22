using System;
using System.Collections.Generic;
using BookStory.Dtos.Base;

namespace BookStory.Dtos
{
    public class AuthorWithBooksDto : BaseDto
    {
        public string Name { get; set; }
    
        public DateTime BirthDate { get; set; }
    
        public IEnumerable<BookDto> Books { get; set; }
    }
}

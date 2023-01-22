using System.Collections.Generic;

namespace BookStory.Domain
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}


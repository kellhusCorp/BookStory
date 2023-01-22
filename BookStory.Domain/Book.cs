#nullable enable
using System.Collections.Generic;
using BookStory.Domain.Details;

namespace BookStory.Domain
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
    
        public int PublishYear { get; set; }
    
        public int GenreId { get; set; }
    
        public Genre? Genre { get; set; }

        public ICollection<AuthorShip> AuthorShips { get; set; }

        public string EditorialOfficeName { get; set; }
    
        public void UpdateDetails(BookDetails details)
        {
            Name = details.Name;
            PublishYear = details.PublishYear;
            GenreId = details.GenreId;
            EditorialOfficeName = details.EditorialOfficeName;
        }
    }
}

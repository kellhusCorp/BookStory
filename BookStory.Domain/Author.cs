using System;
using System.Collections.Generic;
using BookStory.Domain.Details;

namespace BookStory.Domain
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
    
        public DateTime BirthDate { get; set; }
    
        public ICollection<AuthorShip> AuthorShips { get; set; }

        public void UpdateDetails(AuthorDetails details)
        {
            Name = details.Name;
            BirthDate = details.BirthDate;
        }
    }
}


using System;

namespace BookStory.Domain.Details
{
    public struct AuthorDetails
    {
        public string Name { get; }
    
        public DateTime BirthDate { get; }

        public AuthorDetails(string name, DateTime birthDate)
        {
            Name = name;
            BirthDate = birthDate;
        }
    }
}

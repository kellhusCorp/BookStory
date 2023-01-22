using System;
using System.ComponentModel.DataAnnotations;
using BookStory.Attributes;
using BookStory.Dtos.Base;

namespace BookStory.Dtos
{
    public class AuthorDto : BaseDto
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    
        [Required]
        [OnlyPastDateTime]
        public DateTime BirthDate { get; set; }
    }
}

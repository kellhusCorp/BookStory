using System.ComponentModel.DataAnnotations;
using BookStory.Dtos.Base;

namespace BookStory.Dtos
{
    public class GenreDto : BaseDto
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}

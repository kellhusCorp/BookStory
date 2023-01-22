using System.ComponentModel.DataAnnotations;
using BookStory.Dtos.Base;

namespace BookStory.Dtos
{
    public class BookDto : BaseDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    
        [Required]
        [Range(1, 2023, ErrorMessage = "Год публикации может быть больше {1} и меньше {2}")]
        public int PublishYear { get; set; }
    
        [Required]
        public int GenreId { get; set; }
    
        [Required]
        [StringLength(100)]
        public string EditorialOfficeName { get; set; }
    }
}

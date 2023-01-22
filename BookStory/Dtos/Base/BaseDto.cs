using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;


namespace BookStory.Dtos.Base
{
    public class BaseDto
    {
        [BindNever]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
    }
}


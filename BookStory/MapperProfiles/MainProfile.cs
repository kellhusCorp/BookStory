using AutoMapper;
using BookStory.Domain;
using BookStory.Dtos;

namespace BookStory.MapperProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Author, AuthorDto>();

            CreateMap<AuthorDto, Author>()
                .ForMember(x => x.Id, act => act.Ignore());

            CreateMap<Genre, GenreDto>();

            CreateMap<Book, BookDto>();

            CreateMap<BookDto, Book>()
                .ForMember(x => x.Id, act => act.Ignore());

            CreateMap<AuthorShip, AuthorShipDto>()
                .ReverseMap();
        }
    }
}
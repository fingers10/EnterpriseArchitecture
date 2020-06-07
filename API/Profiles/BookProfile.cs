using AutoMapper;
using Fingers10.EnterpriseArchitecture.API.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;

namespace Fingers10.EnterpriseArchitecture.API.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.Value))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description.Value));

            CreateMap<Book, BookForUpdateDto>()
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.Value))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description.Value));
        }
    }
}

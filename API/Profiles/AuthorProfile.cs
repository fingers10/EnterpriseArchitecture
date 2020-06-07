using AutoMapper;
using Fingers10.EnterpriseArchitecture.API.Helpers;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;

namespace Fingers10.EnterpriseArchitecture.API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.Name.First} {src.Name.Last}"))
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.Value.GetCurrentAge()))
                .ForMember(
                    dest => dest.MainCategory,
                    opt => opt.MapFrom(src => src.MainCategory.Value));
        }
    }
}

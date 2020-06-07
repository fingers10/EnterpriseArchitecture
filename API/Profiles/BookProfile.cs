using AutoMapper;
using Fingers10.EnterpriseArchitecture.API.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;

namespace Fingers10.EnterpriseArchitecture.API.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
        }
    }
}

using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;

namespace Fingers10.EnterpriseArchitecture.API.Dtos
{
    public class RegisterStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long NameSuffixId { get; set; }
        public string Email { get; set; }
        public long FavoriteCourseId { get; set; }
        public Grade FavoriteCourseGrade { get; set; }
    }
}

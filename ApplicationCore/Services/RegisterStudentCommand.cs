using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class RegisterStudentCommand : ICommand<Student>
    {
        public RegisterStudentCommand(string firstName, string lastName, long nameSuffixId, string email,
            long favoriteCourseId, Grade favoriteCourseGrade)
        {
            FirstName = firstName;
            LastName = lastName;
            NameSuffixId = nameSuffixId;
            Email = email;
            FavoriteCourseId = favoriteCourseId;
            FavoriteCourseGrade = favoriteCourseGrade;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long NameSuffixId { get; set; }
        public string Email { get; set; }
        public long FavoriteCourseId { get; }
        public Grade FavoriteCourseGrade { get; }

        [AuditLog]
        [DatabaseRetry]
        internal sealed class RegisterCommandHandler : ICommandHandler<RegisterStudentCommand, Student>
        {
            private readonly IAsyncRepository<Student> _studentRepository;

            public RegisterCommandHandler(IAsyncRepository<Student> studentRepository)
            {
                _studentRepository = studentRepository;
            }

            public async Task<Result<Student>> Handle(RegisterStudentCommand command)
            {
                Course favoriteCourse = Course.FromId(command.FavoriteCourseId);
                if (favoriteCourse == null)
                    return Result.Failure<Student>("Course not found");

                Suffix suffix = Suffix.FromId(command.NameSuffixId);
                if (suffix == null)
                    return Result.Failure<Student>("Suffix not found");

                Result<Email> emailResult = Entities.Email.Create(command.Email);
                if (emailResult.IsFailure)
                    return Result.Failure<Student>(emailResult.Error);

                Result<Name> nameResult = Name.Create(command.FirstName, command.LastName, suffix);
                if (nameResult.IsFailure)
                    return Result.Failure<Student>(nameResult.Error);

                var student = new Student(
                    nameResult.Value,
                    emailResult.Value,
                    favoriteCourse,
                    command.FavoriteCourseGrade);

                await _studentRepository.AddAsync(student);

                await _studentRepository.SaveChangesAsync();

                return Result.Success(student);
            }
        }
    }
}

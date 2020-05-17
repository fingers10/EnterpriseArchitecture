using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetListQuery : IQuery<List<Student>>
    {
        public string EnrolledIn { get; }
        public int? NumberOfCourses { get; }

        public GetListQuery(string enrolledIn, int? numberOfCourses)
        {
            EnrolledIn = enrolledIn;
            NumberOfCourses = numberOfCourses;
        }

        internal sealed class GetListQueryHandler : IQueryHandler<GetListQuery, List<Student>>
        {
            private readonly QueriesConnectionString _connectionString;

            public GetListQueryHandler(QueriesConnectionString connectionString)
            {
                _connectionString = connectionString;
            }

            public List<Student> Handle(GetListQuery query)
            {
                //string sql = @"
                //    SELECT s.StudentID Id, s.Name, s.Email,
                //     s.FirstCourseName Course1, s.FirstCourseCredits Course1Credits, s.FirstCourseGrade Course1Grade,
                //     s.SecondCourseName Course2, s.SecondCourseCredits Course2Credits, s.SecondCourseGrade Course2Grade
                //    FROM dbo.Student s
                //    WHERE (s.FirstCourseName = @Course
                //      OR s.SecondCourseName = @Course
                //      OR @Course IS NULL)
                //        AND (s.NumberOfEnrollments = @Number
                //            OR @Number IS NULL)
                //    ORDER BY s.StudentID ASC";

                //using (SqlConnection connection = new SqlConnection(_connectionString.Value))
                //{
                //    List<StudentDto> students = connection
                //        .Query<StudentDto>(sql, new
                //        {
                //            Course = query.EnrolledIn,
                //            Number = query.NumberOfCourses
                //        })
                //        .ToList();

                //    return students;
                //}

                return new List<Student>();
            }
        }
    }
}

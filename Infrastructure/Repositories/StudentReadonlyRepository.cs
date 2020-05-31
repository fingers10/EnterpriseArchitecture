using Dapper;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Repositories
{
    public class StudentReadonlyRepository : IStudentReadonlyRepository
    {
        private readonly IDbConnection _db;

        public StudentReadonlyRepository(QueriesConnectionString connectionString)
        {
            _db = new SqlConnection(connectionString?.Value ?? throw new ArgumentNullException(nameof(connectionString)));
        }

        public async Task<List<StudentDto>> GetAllAsync()
        {
            const string sql = @"
            SELECT S.StudentID Id, CONCAT(S.FirstName, ' ', S.LastName) Name, S.Email, C.Name 
            FROM Student S INNER JOIN Course C ON C.CourseID = S.FavoriteCourseID";

            var students = await _db.QueryAsync<StudentDto>(sql);

            return students.ToList();
        }
    }
}

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
    public class AuthorReadonlyRepository : IAuthorReadonlyRepository
    {
        private readonly IDbConnection _db;

        public AuthorReadonlyRepository(QueriesConnectionString connectionString)
        {
            _db = new SqlConnection(connectionString?.Value ?? throw new ArgumentNullException(nameof(connectionString)));
        }

        public async Task<List<AuthorDto>> GetAllAsync()
        {
            const string sql = @"
            SELECT Id, CONCAT(FirstName, ' ', LastName) Name, 
            YEAR(CONVERT(varchar, GETDATE(), 23))- YEAR(CONVERT(VARCHAR, DateOfBirth, 23)) Age, 
            MainCategory from Author";

            var authors = await _db.QueryAsync<AuthorDto>(sql);

            return authors.ToList();
        }

        public async Task<List<AuthorDto>> GetAllAsync(string mainCategory)
        {
            string sql = @"
            SELECT Id, CONCAT(FirstName, ' ', LastName) Name, 
            YEAR(CONVERT(varchar, GETDATE(), 23))- YEAR(CONVERT(VARCHAR, DateOfBirth, 23)) Age, 
            MainCategory from Author";

            if (!string.IsNullOrWhiteSpace(mainCategory))
            {
                sql += " WHERE MainCategory = @mainCategory";
            }

            var authors = await _db.QueryAsync<AuthorDto>(sql, new { mainCategory });

            return authors.ToList();
        }

        public async Task<AuthorDto> GetAuthorAsync(long id)
        {
            const string sql = @"
            SELECT Id, CONCAT(FirstName, ' ', LastName) Name, 
            YEAR(CONVERT(varchar, GETDATE(), 23))- YEAR(CONVERT(VARCHAR, DateOfBirth, 23)) Age, 
            MainCategory from Author
            WHERE Id = @id";

            return await _db.QuerySingleOrDefaultAsync<AuthorDto>(sql, new { id });
        }
    }
}

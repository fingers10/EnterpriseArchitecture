﻿using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.Infrastructure.Data;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Repositories
{
    public class AuthorRepository : GenericEFRepository<Author, Fingers10Context>
    {
        public AuthorRepository(Fingers10Context context) : base(context)
        {
        }
    }
}

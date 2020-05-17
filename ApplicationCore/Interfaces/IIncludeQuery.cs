using Fingers10.EnterpriseArchitecture.ApplicationCore.Helpers.Query;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface IIncludeQuery
    {
        Dictionary<IIncludeQuery, string> PathMap { get; }
        IncludeVisitor Visitor { get; }
        HashSet<string> Paths { get; }
    }

    public interface IIncludeQuery<TEntity, out TPreviousProperty> : IIncludeQuery
    {
    }
}

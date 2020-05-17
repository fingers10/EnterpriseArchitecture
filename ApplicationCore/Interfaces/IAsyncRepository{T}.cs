using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface IAsyncRepository<T> where T : Entity, IAggregateRoot
    {
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<T> FindAsync(long id);
        Task<T> FirstAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        Task SaveChangesAsync();
        T Update(T entity);
        Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate);
    }
}

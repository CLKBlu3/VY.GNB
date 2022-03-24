using System.Collections.Generic;
using System.Threading.Tasks;

namespace VY.GNB.Infrastructure.Contracts.Repositories
{
    public interface IRepository<T>
    {
        public Task AddAsync(IEnumerable<T> entity);

        public Task<IEnumerable<T>> GetAllAsync();

        public IEnumerable<T> GetById(string name);

        public Task SaveChangesAsync();
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VY.GNB.XCutting.Contracts.Services
{
    public interface IProxy<T>
    {
        public Task<IEnumerable<T>> GetAsync();
    }
}

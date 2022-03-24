using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VY.GNB.Infrastructure.Contracts.Entities;
using VY.GNB.Infrastructure.Contracts.Repositories;
using VY.GNB.Infrastructure.Implementation.Context;

namespace VY.GNB.Infrastructure.Implementation.Repositories
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly TransactionsContext _context;
        private readonly DbSet<Transaction> _dbSet;

        public TransactionRepository(TransactionsContext context)
        {
            _context = context;
            _dbSet = _context.Transactions;
        }

        public async Task AddAsync(IEnumerable<Transaction> entity)
        {
            //Remove everything before adding new values
            var entityList = await GetAllAsync();
            Delete(entityList);
            await _dbSet.AddRangeAsync(entity);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        private void Delete(IEnumerable<Transaction> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IEnumerable<Transaction> GetById(string name)
        {
            return _dbSet.Where(x => x.Name.Equals(name)).ToList();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

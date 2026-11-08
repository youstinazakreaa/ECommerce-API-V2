using onine.core.Entites;
using onine.core.Repositories;
using Microsoft.EntityFrameworkCore;

using onine.core.Specifications;
using onine.Repository.Data;

namespace onine.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly Oninecontext _dbContext;
        public GenericRepository(Oninecontext dbContext)
        {
            _dbContext = dbContext;


        }
        #region without specification
        public async Task<IEnumerable<T>> GetAllwithspecAsync()
        
            =>await _dbContext.Set<T>().ToListAsync();


        public async Task<T> GetById(int id)

           => await _dbContext.Set<T>().FindAsync(id);
        #endregion

        #region with specification
        public async Task<IEnumerable<T>> GetAllwithspecAsync(ISpecifications<T> spec)
        {
            return await Applyspecification(spec).ToListAsync();
            
        }


        public async Task<T> GetByIdwithspecAsync(ISpecifications<T> spec)
        {
            return await Applyspecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> Applyspecification (ISpecifications <T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public Task GetAll()
        {
            throw new NotImplementedException();
        }
       async Task<int> IGenericRepository<T>.GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await GetCountWithSpecAsync(Spec);
        
         }
        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            var query = SpecificationEvaluator<T> .GetQuery(_dbContext.Set<T>(), Spec);
            return await query.CountAsync();
        }
        // Add, Update, Delete methods
        public async Task  AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            _dbContext.SaveChanges();
        }
        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

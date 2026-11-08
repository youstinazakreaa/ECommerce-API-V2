using onine.core.Entites;
using onine.core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onine.core.Repositories
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        #region withoutspec
        Task<IEnumerable<T>> GetAllwithspecAsync();
        Task <T> GetById(int id);
        #endregion



        #region withspec 
        public Task<IEnumerable<T>> GetAllwithspecAsync(ISpecifications<T>spec);
        public Task<T> GetByIdwithspecAsync(ISpecifications<T>spec);
        Task GetAll();

        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        // Task<Action<global::AutoMapper.IMappingOperationOptions<object, void>>> GetByIdAsync(int productId);
        #endregion
    }
}   

using AIRS.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace AIRS.SharedLibrary.Interface;
internal interface IGeneric<T> where T : class
{
    Task<Response<T>> CreateAsync(T entity);
    Task<Response<T>> UpdateAsync(T entity);
    Task<Response<bool>> DeleteAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
}

using System.Linq.Expressions;

namespace GymApi.Domain.Interfaces
{
    public interface IGenericRepository <TEntity> where TEntity : class
    {
        Task<TEntity> Obtain(Expression<Func<TEntity, bool>> filtro);
        Task<IQueryable<TEntity>> Consult(Expression<Func<TEntity, bool>> filtro = null);
        Task<TEntity> Create(TEntity entidad);
        Task<bool> Edit(TEntity entidad);
        Task<bool> Eliminate(TEntity entidad);
        Task<bool> EliminateRange<TEntidad>(Expression<Func<TEntity, bool>> filtro) where TEntidad : class;
    }
}
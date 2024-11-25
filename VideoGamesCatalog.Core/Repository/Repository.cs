using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VideoGamesCatalog.Core.Data.Context;
using VideoGamesCatalog.Core.Data.Models;

namespace VideoGamesCatalog.Core.Data.Repository
{
    public interface IRepository<T> where T: class, IDataEntity
    {
        Task<T> GetByID(int id, string includeProperties = "");
        Task<List<T>> Get(
            List<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? offset = null,
            int? limit = null);
        Task<int> Count(
            List<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? offset = null,
            int? limit = null);
        Task Insert(T entity);
        void Update(T entity);
        void DeleteByID(int id);
        void Delete(T entity);
    
    }
    public class Repository<T> : IRepository<T> where T : class, IDataEntity
    {
        private readonly VideoGameContext _context;
        private readonly DbSet<T> _dbSet;
       
        public Repository(VideoGameContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public virtual void DeleteByID(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        private IQueryable<T> ConstructFilteredQuery(
            List<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? offset = null,
            int? limit = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            foreach (var includeProperty in includeProperties.Split
                ([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (offset != null)
            {
                query = query.Skip(offset.Value);
            }

            if (limit != null)
            {
                query = query.Take(limit.Value);
            }

            return query;
        }

        public async Task<List<T>> Get(
            List<Expression<Func<T, bool>>> filters = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includeProperties = "", 
            int? offset = null, 
            int? limit = null)
        {
            var query = ConstructFilteredQuery(filters, orderBy, includeProperties, offset, limit);
            return await query.ToListAsync<T>();
        }

        public async Task<int> Count(
            List<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? offset = null,
            int? limit = null)
        {
            var query = ConstructFilteredQuery(filters, orderBy,includeProperties, offset, limit);
            return await query.CountAsync();
        }

        public async Task<T> GetByID(int id, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            
            foreach (var includeProperty in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
           
            return await query.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id) != null;
        }

        public async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;
        }

    }
}

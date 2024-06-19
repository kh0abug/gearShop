using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Readonlys

        private readonly ApplicationDbContext _db;
        internal readonly DbSet<T> dbSet;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="db">The Database Context</param>
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The entity that was added</returns>
        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="predicate">The condition of the entity to retrieve</param>
        /// <returns>The entity object if found, otherwise null</returns>
        public Task<T> Get(Expression<Func<T, bool>> predicate, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(predicate);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }
                , StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns><see cref="Task"/></returns>
        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entities">The entities to delete</param>
        /// <returns><see cref="Task"/></returns>
        public async Task RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Gets a collection of all entities.
        /// </summary>
        /// <returns>A collection of all entities</returns>
        public async Task<IQueryable<T>> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.AsNoTracking();
        }

        #endregion
    }
}

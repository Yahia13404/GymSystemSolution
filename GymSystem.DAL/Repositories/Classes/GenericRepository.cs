using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext dpContext;
        public GenericRepository(GymDbContext dbContext)
        {
            dpContext = dbContext;

        }
        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var entities = isTracked ? dpContext.Set<TEntity>() : dpContext.Set<TEntity>().AsNoTracking();
            return await entities.ToListAsync();
        }
        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
        {
            var entity = await dpContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, ct);
            return entity;
        }
        public void Add(TEntity entity)
        {
            dpContext.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            dpContext.Set<TEntity>().Update(entity);
        }
        public void Delete(int id)
        {
            var entity = dpContext.Set<TEntity>().FirstOrDefault(e => e.Id == id);
            if (entity is not null)
            {
                dpContext.Remove(entity);
            }
        }
        public async Task<int> ComoleteAsync()
        {
            return await dpContext.SaveChangesAsync();
        }

        public async Task<TEntity?> FirstOrDefultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            var entity = isTracked ? dpContext.Set<TEntity>() : dpContext.Set<TEntity>().AsNoTracking();
            return await entity.FirstOrDefaultAsync(predicate, ct);

        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            return await dpContext.Set<TEntity>().AnyAsync(predicate, ct);
        }
    }
}

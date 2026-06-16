using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> _Repos = [];


        public UnitOfWork(GymDbContext dbContext)  
        {
            _dbContext = dbContext;
            SessionRepository = new SessionRepository(_dbContext);

        }
        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if(_Repos.TryGetValue(TypeName, out object OldRepo))
            {
                return (IGenericRepository<TEntity>) OldRepo;
            }
            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            _Repos[TypeName] = NewRepo;
            return NewRepo;  

        }
        public async Task<int> CompleteAsync()
        {
          return await _dbContext.SaveChangesAsync();
        }

       
    }
}

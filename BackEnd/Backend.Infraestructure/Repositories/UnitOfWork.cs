using Backend.Application.Contracts.Identity;
using Backend.Application.Contracts.Persistence;
using Backend.Application.Domain.Common;
using Backend.Infraestructure.Persistence;
using System.Collections;

namespace Backend.infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly ShopDbContext _context;
        private readonly IAuthService _authService;


        public UnitOfWork(ShopDbContext context, IAuthService authService)
        {
            _authService = authService;
            _context = context;
        }

        public ShopDbContext AsisYaContext => _context;

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : Entity
        {
            if (_repositories == null)
                _repositories = new Hashtable();
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), new object[] { _context, _authService });
                _repositories.Add(type, repository);

            }

            return (IRepository<TEntity>)_repositories[type];
        }
    }
}

using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.Persistence;
using System.Collections;

namespace Ecommerce.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable? _repositories;

    private readonly ECommerceDbContext _context;

    public UnitOfWork(ECommerceDbContext context)
    {
        _context = context;
    }

    public async Task<int> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Error in transaction", e);
        }
    }

    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        _repositories ??= new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IAsyncRepository<TEntity>)_repositories[type]!;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
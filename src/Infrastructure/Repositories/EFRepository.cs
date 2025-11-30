namespace GishnizApp.Persistence.Repositories;

public class EFRepository<TEntity> : IEFRepository<TEntity>
     where TEntity : class
{
    private readonly AppDbContext _context;
    public DbSet<TEntity> _dbSet { get; }

    public EFRepository(AppDbContext dbContext)
    {
        _context = dbContext;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> Table => _dbSet;
    public virtual IQueryable<TEntity> TableNoTracking => _dbSet.AsNoTracking();

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_context.Remove(entity).Entity);
    }

    public async Task SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_dbSet.Update(entity).Entity);
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AnyAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
        }
        return null;
    }

    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsQueryable();
    }
}

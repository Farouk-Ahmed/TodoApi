using Domain.Interface;
using Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repository.Implemant
{
	public class Repository<T> : IRepository<T> where T : class, IEntityBase, new()
	{
		private readonly DbContext _context;

		public Repository(DbContext context)
		{
			_context = context;
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public async Task<IEnumerable<T>> GetAllAsync(int id)
		{
			IQueryable<T> query = _context.Set<T>();

			return await query.ToListAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			IQueryable<T> query = _context.Set<T>();

			return await query.ToListAsync();
		}

		public async Task<int> AddAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			return 1;
		}

		public async Task<int> UpdateAsync(int id, T entity)
		{
			var existingEntity = await _context.Set<T>().FindAsync(id);

			if (existingEntity != null)
			{
				_context.Entry(existingEntity).State = EntityState.Detached;
			}

			_context.Entry(entity).State = EntityState.Modified;
			return entity.Id > 0 ? 1 : 0;
		}

		public async Task<int> DeleteAsync(int id)
		{
			var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

			EntityEntry entityEntry = _context.Entry<T>(entity);

			entityEntry.State = EntityState.Deleted;
			return entity.Id > 0 ? 1 : 0;

		}
	}

}

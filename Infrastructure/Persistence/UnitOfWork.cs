using Infrastructure.Context;
using Infrastructure.Repository.Implemant;
using Infrastructure.Repository.Interface;

namespace Infrastructure.Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDBContext _context;
		private ITodoRepository _todoRepository;

		public UnitOfWork(ApplicationDBContext context, ITodoRepository todoRepository)
		{
			_context = context;
			_todoRepository = todoRepository;
		}

		public ITodoRepository TodoRepository => _todoRepository ?? new TodoRepository(_context);


		public async Task<int> CompleteAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}

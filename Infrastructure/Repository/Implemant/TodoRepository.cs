using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Implemant
{
	public class TodoRepository : Repository<Todo>, ITodoRepository
	{
		private readonly ApplicationDBContext _context;

		public TodoRepository(ApplicationDBContext context) : base(context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Todo>> GetTodosByUserId(int userId)
		{
			return await _context.Todos
				.Where(todo => todo.CustomUserId == userId)
				.OrderBy(todo => todo.Id)
				.ToListAsync();
		}
	}
}

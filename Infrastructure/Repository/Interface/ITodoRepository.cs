using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interface
{
    public interface ITodoRepository : IRepository<Todo>
    {
        Task<IEnumerable<Todo>> GetTodosByUserId(int userId);
    }
}

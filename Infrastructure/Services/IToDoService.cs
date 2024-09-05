using Domain.Bases;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IToDoService
    {
        Task<CustomResponse<TodoDTO>> GetTodoByIdAsync(int id);
        Task<CustomResponse<IEnumerable<TodoDTO>>> GetAllTodosAsync(int userId);
        Task<CustomResponse<TodoDTO>> GetTodoByIdAsync(int id, int userId);
        Task<CustomResponse<int>> CreateTodoAsync(TodoDTO todoDTO);
        Task<CustomResponse<int>> UpdateTodoAsync(int id, TodoDTO todoDTO);
        Task<CustomResponse<bool>> DeleteTodoAsync(int id);
    }
}

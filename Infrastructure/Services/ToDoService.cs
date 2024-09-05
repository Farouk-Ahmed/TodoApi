using AutoMapper;
using Domain.Bases;
using Domain.DTO;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services
{
	public class ToDoService : IToDoService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _accessor;

		public ToDoService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor accessor)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_accessor = accessor;
		}

		public async Task<CustomResponse<IEnumerable<TodoDTO>>> GetAllTodosAsync(int userId)
		{
			var todos = await _unitOfWork.TodoRepository.GetTodosByUserId(userId);
			var todoDTOs = _mapper.Map<IEnumerable<TodoDTO>>(todos);
			return new CustomResponse<IEnumerable<TodoDTO>>(todoDTOs, "Todos successfully");
		}

		public async Task<CustomResponse<TodoDTO>> GetTodoByIdAsync(int id, int userId)
		{
			var todo = await _unitOfWork.TodoRepository.GetByIdAsync(id);
			if (todo == null || todo.CustomUserId != userId)
			{
				return new CustomResponse<TodoDTO>("Todo not found or unauthorized", false);
			}

			var todoDTO = _mapper.Map<TodoDTO>(todo);
			return new CustomResponse<TodoDTO>(todoDTO, "Todo successfully");
		}


		public async Task<CustomResponse<int>> CreateTodoAsync(TodoDTO todoDTO)
		{
			try
			{
				if (todoDTO == null)
				{
					return new CustomResponse<int>("Invalid input data", false);
				}


				var accessToken = _accessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

				var userId = ExtractUserIdFromToken(accessToken ?? "");
				var todo = _mapper.Map<TodoDTO, Todo>(todoDTO);
				todo.CustomUserId = userId.Value;
				if (userId == null)
				{
					return new CustomResponse<int>("Invalid token or user not authenticated", false);
				}
				var result = await _unitOfWork.TodoRepository.AddAsync(todo);
				await _unitOfWork.CompleteAsync();
				return new CustomResponse<int>(result, result > 0 ? "Todo created successfully" : "Failed to create todo");
			}
			catch (Exception ex)
			{
				return new CustomResponse<int>(ex.Message, false);
			}
		}

		public async Task<CustomResponse<int>> UpdateTodoAsync(int id, TodoDTO todoDTO)
		{
			try
			{
				if (todoDTO == null)
				{
					return new CustomResponse<int>("Invalid input data", false);
				}

				var existingTodo = await _unitOfWork.TodoRepository.GetByIdAsync(id);
				if (existingTodo == null)
				{
					return new CustomResponse<int>("Todo not found", false);
				}

				todoDTO.Id = existingTodo.Id;
				todoDTO.CustomUserId = existingTodo.CustomUserId;
				_mapper.Map(todoDTO, existingTodo);

				await _unitOfWork.TodoRepository.UpdateAsync(id, existingTodo);
				await _unitOfWork.CompleteAsync();

				return new CustomResponse<int>(existingTodo.Id, "Todo updated successfully");
			}
			catch (Exception ex)
			{
				return new CustomResponse<int>(ex.Message, false);
			}
		}

		public async Task<CustomResponse<bool>> DeleteTodoAsync(int id)
		{
			try
			{
				var todo = await _unitOfWork.TodoRepository.GetByIdAsync(id);
				if (todo == null)
				{
					return new CustomResponse<bool>("Todo not found", false);
				}

				await _unitOfWork.TodoRepository.DeleteAsync(id);
				await _unitOfWork.CompleteAsync();

				return new CustomResponse<bool>(true, "Todo deleted successfully");
			}
			catch (Exception ex)
			{
				return new CustomResponse<bool>(ex.Message, false);
			}
		}


		public async Task<CustomResponse<TodoDTO>> GetTodoByIdAsync(int id)
		{
			try
			{
				if (id <= 0)
				{
					return new CustomResponse<TodoDTO>("You must enter a valid id", false);
				}

				var _todo = await _unitOfWork.TodoRepository.GetByIdAsync(id);
				if (_todo == null)
				{
					return new CustomResponse<TodoDTO>("The entered id is not exist", false);
				}
				TodoDTO todo = _mapper.Map<Todo, TodoDTO>(_todo);
				return new CustomResponse<TodoDTO>(todo);

			}
			catch (Exception ex)
			{

				return new CustomResponse<TodoDTO>(ex.Message, false);
			}
		}

		private int? ExtractUserIdFromToken(string token)
		{
			try
			{
				var handler = new JwtSecurityTokenHandler();
				var jwtToken = handler.ReadJwtToken(token);

				var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");

				if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
				{
					return userId;
				}

				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}

}

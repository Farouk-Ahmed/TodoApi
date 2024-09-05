using Domain.Bases;
using Domain.DTO;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ToDoController : ControllerBase
	{
		private readonly IToDoService _toDoService;

		public ToDoController(IToDoService toDoService)
		{
			_toDoService = toDoService;
		}

		private int GetUserId()
		{
			return int.Parse(User.FindFirst("userId")?.Value);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTodos()
		{
			var userId = GetUserId();
			var Result = await _toDoService.GetAllTodosAsync(userId);

			if (!Result.IsCompleted)
			{
				return BadRequest(new CustomResponse<dynamic>(Result.Message, false));
			}

			return Ok(Result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetTodoById(int id)
		{
			var userId = GetUserId();
			var Result = await _toDoService.GetTodoByIdAsync(id, userId);

			if (!Result.IsCompleted)
			{
				return NotFound(Result);
			}

			return Ok(Result);
		}


		[HttpPost]
		public async Task<IActionResult> CreateTodo([FromBody] TodoDTO todoDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new CustomResponse<dynamic>("Invalid data", false));
			}

			var Result = await _toDoService.CreateTodoAsync(todoDTO);

			if (!Result.IsCompleted)
			{
				return BadRequest(Result);
			}

			return Ok(Result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoDTO todoDTO)
		{
			var userId = GetUserId();
			var Result = await _toDoService.UpdateTodoAsync(id, todoDTO);

			if (!Result.IsCompleted)
			{
				return NotFound(Result);
			}

			return Ok(Result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTodo(int id)
		{
			var userId = GetUserId();
			var Result = await _toDoService.DeleteTodoAsync(id);

			if (!Result.IsCompleted)
			{
				return NotFound(Result);
			}

			return Ok(Result);
		}

	}
}

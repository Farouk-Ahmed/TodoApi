using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LiveTodoController : ControllerBase
	{
		private readonly ILiveTodoService _liveTodoService;

		public LiveTodoController(ILiveTodoService liveTodoService)
		{
			_liveTodoService = liveTodoService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllLiveTodosAsync(int page = 1, int pageSize = 10)
		{
			var todos = await _liveTodoService.GetAllLiveTodosAsync(page, pageSize);
			return Ok(todos);
		}

	}
}

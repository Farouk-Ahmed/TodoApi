using Domain.Bases;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
	public class ErrorHandelMiddleware
	{
		private readonly RequestDelegate _next;

		public ErrorHandelMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception error)
			{
				var response = context.Response;
				response.ContentType = "application/json";
				var responseModel = new CustomResponse<string>() { Message = error?.Message, IsCompleted = false };
				switch (error)
				{
					case UnauthorizedAccessException e:
						responseModel.Message = error.Message;
						response.StatusCode = (int)HttpStatusCode.Unauthorized;
						break;

					case ValidationException e:
						responseModel.Message = error.Message;
						response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
						break;
					case KeyNotFoundException e:
						responseModel.Message = error.Message; ;
						response.StatusCode = (int)HttpStatusCode.NotFound;
						break;

					case DbUpdateException e:
						responseModel.Message = e.Message;
						response.StatusCode = (int)HttpStatusCode.BadRequest;
						break;
					case Exception e:
						if (e.GetType().ToString() == "ApiException")
						{
							responseModel.Message += e.Message;
							responseModel.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;
							response.StatusCode = (int)HttpStatusCode.BadRequest;
						}
						responseModel.Message = e.Message;
						responseModel.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;

						response.StatusCode = (int)HttpStatusCode.InternalServerError;
						break;

					default:
						responseModel.Message = error.Message;
						response.StatusCode = (int)HttpStatusCode.InternalServerError;
						break;
				}
				var result = JsonSerializer.Serialize(responseModel);

				await response.WriteAsync(result);
			}
		}
	}
}


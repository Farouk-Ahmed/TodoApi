using Domain.DTO;
using FluentValidation;

namespace Infrastructure.Validation
{
	public class ToDoDTOValidator : AbstractValidator<TodoDTO>
	{
		public ToDoDTOValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required")
				.Length(1, 250).WithMessage("Title must be between 1 and 250 characters");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required");

			RuleFor(x => x.CustomUserId)
				.GreaterThan(0).WithMessage("CustomUserId must be greater than 0");
		}
	}
}

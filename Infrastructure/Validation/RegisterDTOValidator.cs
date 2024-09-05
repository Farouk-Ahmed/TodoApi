using Domain.DTO.Account;
using FluentValidation;

namespace Infrastructure.Validation
{
	public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
	{
		public RegisterDTOValidator()
		{
			RuleFor(x => x.UserName)
				.NotEmpty().WithMessage("User name is required");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(" Email is required")
				.EmailAddress().WithMessage("Invalid email");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required")
				.MinimumLength(6).WithMessage("Password must be least 6 characters");
		}
	}
}

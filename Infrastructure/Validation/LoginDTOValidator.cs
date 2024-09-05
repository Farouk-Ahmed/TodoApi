using Domain.DTO.Account;
using FluentValidation;

namespace Infrastructure.Validation
{
	public class LoginDTOValidator : AbstractValidator<LoginDTO>
	{
		public LoginDTOValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required")
				.EmailAddress().WithMessage("Invalid email");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required");
		}
	}
}

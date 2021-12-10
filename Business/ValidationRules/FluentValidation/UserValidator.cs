using Core.Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3);
            RuleFor(u => u.LastName).NotEmpty().MinimumLength(3);
        }
    }
}

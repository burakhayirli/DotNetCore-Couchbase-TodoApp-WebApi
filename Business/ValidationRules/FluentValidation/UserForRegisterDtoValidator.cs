using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterDtoValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3);
            RuleFor(u => u.LastName).NotEmpty().MinimumLength(3);
            RuleFor(u => u.Password).NotEmpty().MinimumLength(3);
        }
    }
}

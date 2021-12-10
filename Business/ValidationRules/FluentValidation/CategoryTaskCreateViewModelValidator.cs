using Core.Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class CategoryTaskCreateViewModelValidator : AbstractValidator<CategoryTaskCreateViewModel>
    {
        public CategoryTaskCreateViewModelValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Name).NotNull();
            RuleFor(c => c.Name).MinimumLength(3);
            RuleFor(c => c.IsCompleted).NotNull();
        }
    }
}

using FluentValidation;
using Infocity.Api.Domain.FormModels;

namespace Infocity.Api.Infrastructure.Validators;

public class UserUpdateFormModelValidator : AbstractValidator<UserUpdateFormModel>
{
    public UserUpdateFormModelValidator()
    {
        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("Имя отсутствует");

        RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("Фамилия отсутствует");
    }
}
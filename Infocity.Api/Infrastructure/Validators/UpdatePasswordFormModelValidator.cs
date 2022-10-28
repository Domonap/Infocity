using FluentValidation;
using Infocity.Api.Domain.FormModels;
using Infocity.Api.Infrastructure.Helpers;

namespace Infocity.Api.Infrastructure.Validators;

public class UpdatePasswordFormModelValidator : AbstractValidator<UpdatePasswordFormModel>
{
    public UpdatePasswordFormModelValidator()
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .Must(x => PasswordValidator.IsValid(x.Password))
            .WithMessage("Формат пароля не допустим, пожалуйста, используйте цифру, заглавную букву и не менее 6 символов..")
            .Must(x => x.Password == x.RepeatPassword)
            .WithMessage("Повторяющиеся пароли не совпадают");
    }
}
using FluentValidation;
using Infocity.Api.Domain.FormModels;
using Infocity.Api.Infrastructure.Helpers;

namespace Infocity.Api.Infrastructure.Validators;

public class UserCreateFormModelValidator : AbstractValidator<UserCreateFormModel>
{
    public UserCreateFormModelValidator()
    {
        RuleFor(m => m.Username)
            .Cascade(CascadeMode.Stop)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("Имя пользователя отсутствует")
            .MinimumLength(3).WithMessage("Имя пользователя короткое")
            .MaximumLength(25).WithMessage("Имя пользователя слишком длинное")
            .Matches(RegexConstants.NicknameOneChar)
            .WithMessage("Имя пользователя содержит недопустимые символы")
            .Matches(RegexConstants.NicknameRegex)
            .WithMessage("Неверный формат имени пользователя");


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

        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .Must(x => PasswordValidator.IsValid(x.Password))
            .WithMessage("Формат пароля не допустим, пожалуйста, используйте цифру, заглавную букву и не менее 6 символов..")
            .Must(x => x.Password == x.RepeatPassword)
            .WithMessage("Повторяющиеся пароли не совпадают");
    }
}
using Infocity.Api.Domain.FormModels;
using Microsoft.AspNetCore.Mvc;

namespace Infocity.Api.Domain.Contracts;

public interface IUsersService
{
    Task<ObjectResult> CreateUser(UserCreateFormModel model);
    Task<ObjectResult> GetUserModel(string username);
    Task<ObjectResult> UpdateUser(UserUpdateFormModel model, string username);
    Task<IActionResult> ChangeUserPassword(UpdatePasswordFormModel model, string username);
    Task<ObjectResult> DeactivateUser(string username);
}
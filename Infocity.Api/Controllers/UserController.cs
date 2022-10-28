using System.Net;
using Infocity.Api.Domain.Contracts;
using Infocity.Api.Domain.FormModels;
using Infocity.Api.Domain.Models;
using Infocity.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace Infocity.Api.Controllers;

[Authorize]
[Produces("application/json")]
[Route("api/{culture=ru}/[controller]")]
[ProducesResponseType(typeof(ModelStateDictionary), (int) HttpStatusCode.BadRequest)]
[ProducesResponseType(typeof(string), (int) HttpStatusCode.InternalServerError)]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UserController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    /// <summary>
    ///    Создает нового пользователя.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("Create")]
    [SwaggerOperation(Tags = new[] {"User"})]
    [ProducesResponseType(typeof(UserModel), (int) HttpStatusCode.Created)]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateFormModel model)
    {
        return await _usersService.CreateUser(model);
    }

    /// <summary>
    ///     Возвращает информацию о пользователе для определенного имени пользователя
    /// </summary>
    [HttpGet("{username}")]
    [SwaggerOperation(Tags = new[] {"User"})]
    [ProducesResponseType(typeof(UserModel), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserModel([FromRoute] string username)
    {
        return await _usersService.GetUserModel(username);
    }

    /// <summary>
    /// Получает использованную информацию для текущего пользователя
    /// </summary> 
    [HttpGet]
    [SwaggerOperation(Tags = new[] {"User"})]
    [ProducesResponseType(typeof(UserModel), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserModel()
    {
        var username = Request.GetUsername();
        return await _usersService.GetUserModel(username);
    }

    /// <summary>
    /// Обновляет текущего пользователя
    /// </summary> 
    [HttpPut("Update")]
    [SwaggerOperation(Tags = new[] {"User"})]
    [ProducesResponseType(typeof(UserModel), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateFormModel model)
    {
        var username = Request.GetUsername();
        return await _usersService.UpdateUser(model, username);
    }

    /// <summary>
    /// Изменить пароль для текущего пользователя.
    /// После изменения вам нужно будет войти в систему с новым паролем.
    /// </summary> 
    [HttpPatch("ChangePassword")]
    [SwaggerOperation(Tags = new[] {"User"})]
    [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> ChangeUserPassword([FromBody] UpdatePasswordFormModel model)
    {
        var username = Request.GetUsername();
        return await _usersService.ChangeUserPassword(model, username);
    }

    /// <summary>
    ///  Деактивирует пользователя
    /// </summary> 
    [HttpDelete("Deactivate")]
    [SwaggerOperation(Tags = new[] {"User"})]
    [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> DeactivateUser()
    {
        var username = Request.GetUsername();
        return await _usersService.DeactivateUser(username);
    }
}
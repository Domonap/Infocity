using System.Net;
using Infocity.Api.Domain.Contracts;
using Infocity.Api.Domain.Entities;
using Infocity.Api.Domain.Enums;
using Infocity.Api.Domain.FormModels;
using Infocity.Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Infocity.Api.Services;

public class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IObjectResultFactory _response;
    private readonly ILogger<UsersService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ICredentialsProvider _credentialsProvider;

    public UsersService(ILogger<UsersService> logger,
        IUserRepository userRepository,
        IObjectResultFactory objectResultFactory,
        IMemoryCache memoryCache,
        ICredentialsProvider credentialsProvider)
    {
        _userRepository = userRepository;
        _logger = logger;
        _response = objectResultFactory;
        _memoryCache = memoryCache;
        _credentialsProvider = credentialsProvider;
    }

    public async Task<ObjectResult> CreateUser(UserCreateFormModel model)
    {
        try
        {
            if (await UsernameAlreadyRegistered(model.Username.ToLower()))
                return _response[HttpStatusCode.Forbidden, "Имя пользователя недоступно"];


            var newUser = new User
            {
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Credentials = _credentialsProvider.GenerateCredentials(model.Password)
            };


            await _userRepository.AddUser(newUser);


            _logger.LogInformation("{@Username}", model.Username);

            RemoveCache();

            return _response[HttpStatusCode.Created, new UserModel
            {
                Username = newUser.Username,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserStatus = newUser.UserStatus
            }];
        }
        catch (Exception e)
        {
            RemoveCache();
            _logger.LogError("{@Exception} {@Message}", e, "Не удалось создать пользователя");
            return _response[HttpStatusCode.InternalServerError, "Не удалось создать пользователя"];
        }
    }

    public async Task<ObjectResult> UpdateUser(UserUpdateFormModel model, string username)
    {
        try
        {
            var user = await _userRepository.GetUser(username);
            if (user == null) return _response[HttpStatusCode.InternalServerError, "Пользователь не найден"];

            if (!string.IsNullOrWhiteSpace(model.FirstName))
                user.FirstName = model.FirstName;

            if (!string.IsNullOrWhiteSpace(model.LastName))
                user.LastName = model.LastName;

            var updatedUser = await _userRepository.UpdateUser(user);

            return _response[HttpStatusCode.OK, new UserModel
            {
                Username = updatedUser.Username,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                UserStatus = updatedUser.UserStatus
            }];
        }
        catch (Exception e)
        {
            _logger.LogError("{@Exception} {@Message}", e, "Ошибка обновления пользователя");
            return _response[HttpStatusCode.InternalServerError, "Ошибка обновления пользователя"];
        }
    }

    public async Task<IActionResult> ChangeUserPassword(UpdatePasswordFormModel model, string username)
    {
        try
        {
            var user = await _userRepository.GetUser(username);
            if (user == null) return _response[HttpStatusCode.InternalServerError, "Пользователь не найден"];

            var credentials = _credentialsProvider.GenerateCredentials(model.Password);

            user.Credentials = credentials;

            await _userRepository.UpdateUser(user);

            return _response[HttpStatusCode.OK];
        }
        catch (Exception e)
        {
            _logger.LogError("{@Exception} {@Message}", e, "Не удалось изменить пароль");
            return _response[HttpStatusCode.InternalServerError, "Не удалось изменить пароль"];
        }
    }

    public async Task<ObjectResult> GetUserModel(string username)
    {
        try
        {
            var user = await _userRepository.GetUser(username);

            return user == null
                ? _response[HttpStatusCode.InternalServerError, "Пользователь не найден"]
                : _response[HttpStatusCode.OK, new UserModel
                {
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserStatus = user.UserStatus
                }];
        }
        catch (Exception e)
        {
            _logger.LogError("{@Exception} {@Message}", e, "Ошибка извлечения пользователя");
            return _response[HttpStatusCode.InternalServerError, "Ошибка извлечения пользователя"];
        }
    }

    public async Task<ObjectResult> DeactivateUser(string username)
    {
        try
        {
            var user = await _userRepository.GetUser(username);
            if (user == null) return _response[HttpStatusCode.InternalServerError, "Пользователь не найден"];

            user.UserStatus = UserStatus.Deactivated;

            await _userRepository.UpdateUser(user);

            RemoveCache();

            return _response[HttpStatusCode.OK];
        }
        catch (Exception e)
        {
            _logger.LogError("{@Exception} {@Message}", e, "Не удалось деактивировать пользователя");
            return _response[HttpStatusCode.InternalServerError, "Не удалось деактивировать пользователя"];
        }
    }

    private async Task<bool> UsernameAlreadyRegistered(string username)
    {
        var names = await _userRepository.GetRegisteredUsernames();
        return names.Any(x => x.Equals(username, StringComparison.InvariantCultureIgnoreCase));
    }

    private void RemoveCache()
    {
        _memoryCache.Remove("_usernames");
    }
}
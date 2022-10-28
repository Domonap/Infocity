namespace Infocity.Api.Domain.FormModels;

public class UserCreateFormModel
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }
}
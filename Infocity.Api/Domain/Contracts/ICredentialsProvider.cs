using Infocity.Api.Domain.Entities;

namespace Infocity.Api.Domain.Contracts;

public interface ICredentialsProvider
{
    Credentials GenerateCredentials(string password);
    string HashPassword(string password, string salt);
}
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Infocity.Api.Domain.Contracts;

public interface IObjectResultFactory
{
    ObjectResult this[HttpStatusCode code] { get; }
    ObjectResult this[HttpStatusCode code, object obj] { get; }
    ObjectResult this[HttpStatusCode code, string message] { get; }
}
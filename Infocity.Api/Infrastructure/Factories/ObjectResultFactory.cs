using System.Net;
using Infocity.Api.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Infocity.Api.Infrastructure.Factories
{
    public class ObjectResultFactory : IObjectResultFactory
    {
        public ObjectResult this[HttpStatusCode code] => ObjectResult(code);

        public ObjectResult this[HttpStatusCode code, object obj] => ObjectResult(code, obj);

        public ObjectResult this[HttpStatusCode code, string message] => ObjectResult(code, message);


        private ObjectResult ObjectResult(HttpStatusCode statusCode)
        {
            return new ObjectResult(statusCode.ToString())
                {StatusCode = (int) statusCode};
        }

        private ObjectResult ObjectResult(HttpStatusCode statusCode, string message)
        {
            return new ObjectResult(message) {StatusCode = (int) statusCode};
        }

        private ObjectResult ObjectResult(HttpStatusCode statusCode, object obj)
        {
            return new ObjectResult(obj) {StatusCode = (int) statusCode};
        }
    }
}
using System;
using Volta.Core.Domain;

namespace Volta.Core.Application.Security
{
    public class NotInitializedException : Exception { }
    public class AccessDeniedException : Exception { }

    public interface IAuthenticationService
    {
        User Authenticate(string username, string password);
    }
}
using System;

namespace Volta.Core.Domain
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) {}
    }
}
namespace Volta.Core.Domain
{
    public class NotFoundException : ValidationException
    { public NotFoundException(string name) : base(string.Format("{0} not found.", name)) { } }
}
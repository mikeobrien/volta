namespace Volta.Core.Application.Configuration
{
    public interface IConfiguration
    {
        string ConnectionString { get; }
        string ErrorUrl { get; }
        string AccessDeniedUrl { get; }
    }
}
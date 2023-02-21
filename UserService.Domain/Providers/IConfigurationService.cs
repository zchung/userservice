namespace UserService.Domain.Providers
{
    /// <summary>
    /// This interface acts as a wrapper for Configuration
    /// It extends the ability for the application to use a different configuration provider
    /// </summary>
    public interface IConfigurationService
    {
        Task<T> GetValue<T>(string key, T defaultValue);
    }
}
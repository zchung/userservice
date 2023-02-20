namespace UserService.Domain.Providers
{
    public interface IConfigurationService
    {
        Task<T> GetValue<T>(string key, T defaultValue);
    }
}
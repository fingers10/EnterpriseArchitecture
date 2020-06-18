namespace Fingers10.EnterpriseArchitecture.API.Services
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
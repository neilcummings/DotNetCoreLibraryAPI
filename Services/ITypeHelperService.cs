namespace WebApi.Services
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
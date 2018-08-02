namespace Api.Services.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface INumeratorService
    {
        Task<int> GetNextNumer(Type entityType);
    }
}

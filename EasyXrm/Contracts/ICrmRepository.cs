using Microsoft.Xrm.Sdk;

namespace EasyXrm.Contracts
{
    public interface ICrmRepository<TEntity> where TEntity : Entity
    {
    }
}
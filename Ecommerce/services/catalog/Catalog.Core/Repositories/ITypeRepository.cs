using Catalog.Core.Entites;

namespace Catalog.Core.Repositories;

public interface ITypeRepository
{
    Task<IEnumerable<ProductType>> GetAllTypes();
}

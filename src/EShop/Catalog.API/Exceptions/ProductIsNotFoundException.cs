using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions
{
    public class ProductIsNotFoundException : NotFoundException
    {

        public ProductIsNotFoundException(Guid Id): base("Product", Id) 
        {
                
        }
    }
}

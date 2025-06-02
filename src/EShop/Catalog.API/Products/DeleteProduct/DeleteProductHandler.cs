using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.DeleteProduct
{
   
    public record DeleteProductQuery(Guid Id) : IQuery<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    internal class DeleteProductQueryHandler
        (IDocumentSession session, ILogger<DeleteProductQueryHandler> logger)
        : IQueryHandler<DeleteProductQuery, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductQueryHandler.Handle called with {@Query}", query);

           var product = await session.LoadAsync<Product>(query.Id, cancellationToken) ?? throw new ProductIsNotFoundException(query.Id);
            session.Delete(product);

            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}

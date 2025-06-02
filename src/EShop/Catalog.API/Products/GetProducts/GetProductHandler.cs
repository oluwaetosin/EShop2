using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Pagination;
using MediatR;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProducResult>;

    public record GetProducResult(IEnumerable<Product> Products);

    internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) 
        : IQueryHandler<GetProductQuery, GetProducResult>

    {
        async Task<GetProducResult> IRequestHandler<GetProductQuery, GetProducResult>.Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsQueryHandler.HANDLER CALLED WITH {@Query}", query);

            var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

            return new GetProducResult(products);
        }
    }
}

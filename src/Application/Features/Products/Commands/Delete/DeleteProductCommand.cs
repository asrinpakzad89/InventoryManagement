namespace Application.Features.Products.Commands.Delete;

public class DeleteProductCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

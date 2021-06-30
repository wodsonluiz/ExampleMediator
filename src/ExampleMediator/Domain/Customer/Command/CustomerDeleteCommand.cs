using MediatR;

namespace ExampleMediator.Domain.Customer.Command
{
    public class CustomerDeleteCommand : IRequest<string>
    {
        public int Id { get; set; }
    }
}
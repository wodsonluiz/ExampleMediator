using System.Threading;
using System.Threading.Tasks;
using ExampleMediator.Domain.Customer.Command;
using ExampleMediator.Domain.Customer.Entity;
using ExampleMediator.Infra;
using ExampleMediator.Notifications;
using MediatR;

namespace ExampleMediator.Domain.Customer.Handler
{
    public class CustomerHandler : 
        IRequestHandler<CustomerCreateCommand, string>,
        IRequestHandler<CustomerUpdateCommand, string>,
        IRequestHandler<CustomerDeleteCommand, string>
    {
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _customerRespository;

        public CustomerHandler(IMediator mediator, ICustomerRepository customerRepository)
        {
            _mediator = mediator;
            _customerRespository = customerRepository;
        }

        public async Task<string> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        {
             var customer = new CustomerEntity(request.Id, request.FirstName, request.LastName, request.Email, request.Phone);
             await _customerRespository.Save(customer);

             await _mediator.Publish(new CustomerActionNotification 
             {
                 FirstName = request.FirstName,
                 LastName = request.LastName,
                 Email = request.Email,
                 Action = ActionNotification.Criado
             }, cancellationToken);

             return await Task.FromResult("Cliente registrado com sucesso");
        }

        public async Task<string> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            var customer = new CustomerEntity(request.Id, request.FirstName, request.LastName, request.Email, request.Phone);
            await _customerRespository.Update(request.Id, customer);

            await _mediator.Publish(new CustomerActionNotification 
             {
                 FirstName = request.FirstName,
                 LastName = request.LastName,
                 Email = request.Email,
                 Action = ActionNotification.Atualizado
             }, cancellationToken);

             return await Task.FromResult("Cliente atualizado com sucesso");

        }

        public async Task<string> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        {
            var client = await _customerRespository.GetById(request.Id);
            await _customerRespository.Delete(request.Id);

            await _mediator.Publish(new CustomerActionNotification 
             {
                 FirstName = client.FirstName,
                 LastName = client.LastName,
                 Email = client.Email,
                 Action = ActionNotification.Excluido
             }, cancellationToken);

             return await Task.FromResult("Cliente excluido com sucesso");

        }
            
    }
}
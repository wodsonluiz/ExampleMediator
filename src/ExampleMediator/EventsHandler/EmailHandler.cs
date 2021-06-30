using System;
using System.Threading;
using System.Threading.Tasks;
using ExampleMediator.Notifications;
using MediatR;

namespace ExampleMediator.EventsHandler
{
    public class EmailHandler : INotificationHandler<CustomerActionNotification>
    {
        public Task Handle(CustomerActionNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                Console.WriteLine("O Cliente {0} {1} foi {2} com sucesso", notification.FirstName, notification.LastName, notification.Action.ToString().ToLower());
            });
        }
    }
}
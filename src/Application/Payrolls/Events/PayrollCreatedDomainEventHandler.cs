using Domain.Payrolls;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Payrolls.Events;

internal sealed class PayrollCreatedDomainEventHandler(
    ILogger<PayrollCreatedDomainEventHandler> logger)
    : INotificationHandler<PayrollCreatedDomainEvent>
{
    public Task Handle(PayrollCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Payroll created event received for PayrollId {PayrollId}",
            notification.PayrollId);

        return Task.CompletedTask;
    }
}

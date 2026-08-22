

namespace EventBus.Messages.Events;

public class BaseIntegrationEvents
{
    public string CorrelationId {  get; set; }
    public DateTime CreationDate { get; set; }

    public BaseIntegrationEvents()
    {
        CorrelationId = Guid.NewGuid().ToString();
        CreationDate = DateTime.Now;
    }
    public BaseIntegrationEvents(Guid correlationId, DateTime creationDate)
    {
        CorrelationId = correlationId.ToString();
        CreationDate = creationDate;
    }
}

using Guuyb.OutboxMessaging.Data.Models;

namespace Commentaries.Infrastructure.SecondaryAdapters.Db.Models;

public class OutboxMessage : IOutboxMessage
{
    public DateTime CreatedAt { get; set; }
    public int Id { get; set; }
    public byte[] Payload { get; set; } = new byte[0];
    public string PayloadTypeName { get; set; } = string.Empty;
    public int PublishAttemptCount { get; set; }
    public DateTime? PublishedAt { get; set; }
    public OutboxMessageStateEnum StateId { get; set; }
    public string? TargetQueueName { get; set; }
    public string? RoutingKey { get; set; }
    public string? ParentActivityId { get; set; }
    public DateTime? DelayUntil { get; set; }
}

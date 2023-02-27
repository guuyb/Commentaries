using Guuyb.OutboxMessaging.Data.Models;

namespace Commentaries.Infrastructure.SecondaryAdapters.Db.Models;

public class OutboxMessageState : IOutboxMessageState
{
    public OutboxMessageStateEnum Id { get; set; }
    public string Code { get; set; } = string.Empty; 
}

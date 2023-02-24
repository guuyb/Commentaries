using System;
using System.Runtime.Serialization;

namespace Commentaries.Application.Common.Exceptions;

public class NotFoundException : LocalizedException
{
    public NotFoundException(string? localizedMessage, string? message, Exception? innerException = null) : base(innerException, localizedMessage, message)
    {
    }

    public NotFoundException(string localizeEtityName, string entityName, object id, Exception? innerException = null)
        : base(innerException, $"Запись \"{localizeEtityName}\" ({id}) не найдена.", "Entity \"{entityName}\" ({id}) was not found.", entityName, id)
    {
    }

    public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
using System;
using System.Runtime.Serialization;

namespace Commentaries.Application.Common.Exceptions;

public class LocalizedException : Exception
{
    public string? LocalizedMessage { get; }
    public object[] Parameters { get; } = new object[0];


    public LocalizedException(string? localizedMessage, string? message, params object[] parameters) : base(message)
    {
        LocalizedMessage = localizedMessage;
        Parameters = parameters;
    }

    public LocalizedException(Exception? innerException, string? localizedMessage, string? message, params object[] parameters) : base(message, innerException)
    {
        LocalizedMessage = localizedMessage;
        Parameters = parameters;
    }

    protected LocalizedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

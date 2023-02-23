using System.Collections.Generic;
using System.Linq;

namespace Commentaries.Domain.Common.Exceptions;

public class ValidationException : LocalizedException
{
    public List<ErrorMessage> ErrorMessages { get; set; }
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException() : base("Произошла одна или несколько ошибок валидации", "One or more validation failures have occurred.")
    {
        ErrorMessages = new List<ErrorMessage>();
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(params ErrorMessage[] errorMessages) : this()
    {
        ErrorMessages = errorMessages.ToList();
    }

    public ValidationException(IDictionary<string, string[]> errors) : this()
    {
        Errors = errors;
    }

    public ValidationException(string localizeErrorMessage, string errorMessage, params object[] param) : this()
    {
        AddErrorMessage(errorMessage, localizeErrorMessage, param);
    }

    public void AddErrorMessage(string localizedErrorMessage, string errorMessage, params object[] parameters)
    {
        ErrorMessages.Add(
            new ErrorMessage(
                LocalizedMessage: localizedErrorMessage,
                Message: errorMessage,
                Parameters: parameters));
    }

    public record ErrorMessage(
        string Message,
        string LocalizedMessage,
        object[] Parameters);
}

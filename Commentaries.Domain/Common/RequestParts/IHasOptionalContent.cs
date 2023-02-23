namespace Commentaries.Domain.Common.RequestParts;

public interface IHasOptionalContent
{
    string? Content { get; }
}

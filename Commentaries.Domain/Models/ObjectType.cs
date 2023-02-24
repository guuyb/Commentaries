using System;

namespace Commentaries.Domain.Models;

public class ObjectType
{
    public const int FULL_NAME_MAX_LENGTH = 200;

    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Common;

public class CommonOptions
{
    public const string Name = "Common";

    [Required]
    public required string PubsubName { get; init; }

    [Required]
    public required string BindingName { get; init; }

    [Required]
    public required string StateStoreName { get; init; }

    [Required]
    public required string DomainEventTopicName { get; init; }

    [Required]
    public required string MessageTopicName { get; init; }
}

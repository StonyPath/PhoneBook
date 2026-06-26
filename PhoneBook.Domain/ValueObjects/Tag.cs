using System.Text.RegularExpressions;

namespace PhoneBook.Domain.ValueObjects;

public class Tag : IEquatable<Tag>
{
    public string Value { get; }

    public Tag(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tag cannot be empty.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length < 2 || trimmed.Length > 30)
            throw new ArgumentException("Tag must be between 2 and 30 characters.", nameof(value));

        if (!Regex.IsMatch(trimmed, @"^[a-zA-Z0-9\-]+$"))
            throw new ArgumentException("Tag may only contain letters, digits, and hyphens.", nameof(value));

        Value = trimmed.ToLowerInvariant();
    }

    public override bool Equals(object? obj) => Equals(obj as Tag);
    public bool Equals(Tag? other) => other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
    public override string ToString() => Value;
}

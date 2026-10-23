using System.Globalization;

namespace RazorClassLibrary;

internal static class ComponentCss
{
    public static void EnsureElementName(string? value, string componentName)
    {
        if (!IsElementName(value))
        {
            throw new InvalidOperationException($"{componentName}.Element must be an HTML element name such as \"div\", but was '{value}'.");
        }
    }

    /// <summary>
    /// Appends the <c>style</c> attribute supplied by the user, so that its declarations override the component's.
    /// </summary>
    public static string MergeStyle(string componentStyle, IReadOnlyDictionary<string, object>? attributes)
    {
        if (attributes is null || !attributes.TryGetValue("style", out var value))
        {
            return componentStyle;
        }

        var userStyle = Convert.ToString(value, CultureInfo.InvariantCulture);
        return string.IsNullOrWhiteSpace(userStyle) ? componentStyle : $"{componentStyle} {userStyle}";
    }

    private static bool IsElementName(string? value)
    {
        if (string.IsNullOrEmpty(value) || !char.IsAsciiLetter(value[0]))
        {
            return false;
        }

        foreach (var c in value.AsSpan(1))
        {
            if (!(char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.'))
            {
                return false;
            }
        }

        return true;
    }
}

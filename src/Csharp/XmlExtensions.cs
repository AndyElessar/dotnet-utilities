using System.Xml.Linq;

namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension(XElement xElement)
    {
        /// <summary>
        /// Wraps an XML string with a specified root element (<paramref name="rootName"/>).
        /// </summary>
        /// <param name="xml">The XML string to wrap.</param>
        /// <param name="rootName">The root element name. Defaults to "root".</param>
        /// <param name="extra">Additional attributes or declarations for the root element.</param>
        /// <returns>The wrapped <see cref="XElement"/>.</returns>
        public static XElement WrapXml(string xml, string rootName = "root", string? extra = "") =>
            XElement.Parse($"<{rootName} {extra}>{xml}</{rootName}>");

        /// <summary>
        /// Removes the specified root element (<paramref name="rootName"/>) from an XML string.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <param name="rootName">The root element name. Defaults to "root".</param>
        /// <returns>The XML content without the root element.</returns>
        public static string UnwrapXml(string xml, string rootName = "root")
        {
            var xmlSpan = xml.AsSpan();
            if(xmlSpan.StartsWith($"<{rootName}>", StringComparison.Ordinal)
                && xmlSpan.EndsWith($"</{rootName}>", StringComparison.Ordinal))
            {
                var startLength = xmlSpan.IndexOf('>') + 1;
                return xmlSpan.Slice(
                    startLength,
                    xmlSpan.Length - (startLength + rootName.Length + 3)
                ).ToString();
            }
            else if(xmlSpan.StartsWith($"<{rootName}", StringComparison.Ordinal)
                    && xmlSpan.EndsWith("/>", StringComparison.Ordinal))
            {
                return string.Empty;
            }
            else
            {
                return xml;
            }
        }

        /// <summary>
        /// Removes the specified root element (<paramref name="rootName"/>) from the <paramref name="xElement"/>.
        /// </summary>
        /// <param name="rootName">The root element name. Defaults to "root".</param>
        /// <returns>The XML content without the root element.</returns>
        public string UnwrapXml(string rootName = "root")
        {
            string xml = xElement.ToString(SaveOptions.DisableFormatting);
            return UnwrapXml(xml, rootName);
        }
    }

    /// <param name="xns">The XML namespace.</param>
    extension(XNamespace xns)
    {
        /// <summary>
        /// Gets the XML namespace declaration string.
        /// </summary>
        /// <param name="prefix">The namespace prefix.</param>
        /// <returns>The namespace declaration string in the format xmlns:prefix="namespace".</returns>
        public string GetNameSpaceDeclare(string prefix) =>
            $"xmlns:{prefix}=\"{xns.NamespaceName}\"";
    }
}

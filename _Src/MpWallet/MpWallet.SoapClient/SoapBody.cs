using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MpWallet.SoapClient;

public static class SoapBody
{
    public static XElement ToXElement<T>(T input)
        where T : class
    {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<XmlRootAttribute>();

        var @namespace = (XNamespace)(attribute?.Namespace ?? string.Empty);
        var name = attribute?.ElementName ?? type.Name;

        var elements = ConvertProperties(input, @namespace).ToArray<object?>();
        return new XElement(@namespace + name, elements);
    }

    private static IEnumerable<XElement> ConvertProperties<T>(T input, XNamespace @namespace)
        where T : class
    {
        var type = typeof(T);
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            var ignoreAttribute = property.GetCustomAttribute<XmlIgnoreAttribute>();
            if (ignoreAttribute != null)
                continue;
            
            var elementAttribute = property.GetCustomAttribute<XmlElementAttribute>();
            var name = elementAttribute?.ElementName ?? property.Name;
            @namespace = elementAttribute?.Namespace ?? @namespace;

            var value = property.GetValue(input);
            yield return new XElement(@namespace + name, value);
        }
    }
}
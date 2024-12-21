using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MpWallet.SoapClient.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<XDocument> ReadSoapXmlBodyAsync(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        var xml = await response.Content.ReadAsStringAsync(cancellationToken);

        var document = XDocument.Parse(xml);
        XDocument? body;
        if (!TryExtractSoapBody(document, SoapVersion.Soap11, out body) &&
            !TryExtractSoapBody(document, SoapVersion.Soap12, out body))
            throw new InvalidOperationException("Can't extract soap body from response");

        return body;
    }
    
    public static async Task<T> ReadSoapBodyAsync<T>(
        this HttpResponseMessage response, 
        CancellationToken cancellationToken = default)
        where T : class, new()
    {
        var xml = await response.Content.ReadAsStringAsync(cancellationToken);

        var document = XDocument.Parse(xml);
        XDocument? body;
        if (!TryExtractSoapBody(document, SoapVersion.Soap11, out body) &&
            !TryExtractSoapBody(document, SoapVersion.Soap12, out body))
            throw new InvalidOperationException("Can't extract soap body from response");

        var serializer = new XmlSerializer(typeof(T));
        var result = serializer.Deserialize(body.CreateReader());
        
        if (result is null)
            throw new InvalidOperationException("Can't deserialize soap body from response");

        return (T)result;
    }

    private static bool TryExtractSoapBody(
        XDocument document, 
        SoapVersion version, 
        [NotNullWhen(true)] out XDocument? body)
    {
        var @namespace = version.ToXmlNamespace();
        var descendant = document.Descendants(@namespace + "Body").FirstOrDefault();
        
        body = descendant is not null 
            ? new XDocument(descendant.Elements()) 
            : null;
        
        return body is not null;
    }
}
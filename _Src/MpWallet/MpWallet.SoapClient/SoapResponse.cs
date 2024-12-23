using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MpWallet.SoapClient;

public class SoapResponse<TBody>
{
    private readonly XDocument? _body;
    
    public HttpStatusCode StatusCode { get; }

    private SoapResponse(HttpStatusCode statusCode, XDocument? body)
    {
        _body = body;
        
        StatusCode = statusCode;
    }

    public bool TryGetBody([NotNullWhen(true)] out TBody? response)
    {
        response = default;
        if (_body is null)
            return false;
        
        var serializer = new XmlSerializer(typeof(TBody));
        var reader = _body.CreateReader();
        
        var deserialized = serializer.Deserialize(reader);
        if (deserialized is null)
            return false;

        response = (TBody)deserialized;
        return true;
    }

    public static async Task<SoapResponse<TBody>> CreateAsync(
        HttpResponseMessage message, 
        CancellationToken cancellationToken = default)
    {
        var statusCode = message.StatusCode;
        var body = statusCode is HttpStatusCode.OK
            ? await ReadBodyAsync(message, cancellationToken)
            : null;
        
        return new SoapResponse<TBody>(statusCode, body);
    }
    
    public static async Task<XDocument> ReadBodyAsync(
        HttpResponseMessage response,
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
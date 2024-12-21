using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Xml.Linq;

namespace MpWallet.SoapClient;

public sealed class RawSoapClient
{
    private readonly HttpClient _httpClient;

    public RawSoapClient()
    {
        _httpClient = new HttpClient(new SocketsHttpHandler());
    }

    public Task<HttpResponseMessage> PostAsync(
        Uri uri, 
        SoapVersion version, 
        IEnumerable<XElement> bodies, 
        IEnumerable<XElement>? headers = null,
        string? action = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(bodies);

        var mediaType = version.ToMediaType();
        var schema = version.ToXmlNamespace();
        var soapElement = version.ToSoapElement();

        var envelope = new XElement(
            schema + "Envelope",
            new XAttribute(
                XNamespace.Xmlns + soapElement,
                schema.NamespaceName));
        
        if (headers is not null)
            envelope.Add(new XElement(schema + "Header", headers));
        
        envelope.Add(new XElement(schema + "Body", bodies));

        var content = new StringContent(envelope.ToString(), Encoding.UTF8, mediaType);
        
        if (action is not null)
            content.Headers.Add("SOAPAction", action);
        
        return _httpClient.PostAsync(uri, content, cancellationToken);
    }
}
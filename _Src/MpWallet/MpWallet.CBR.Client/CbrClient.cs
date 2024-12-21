using System.Xml.Linq;
using System.Xml.Serialization;
using MpWallet.CBR.Client.Models;
using MpWallet.SoapClient;
using MpWallet.SoapClient.Extensions;

namespace MpWallet.CBR.Client;

public sealed class CbrClient
{
    private readonly RawSoapClient _client;

    public CbrClient()
    {
        _client = new RawSoapClient();
    }

    public async Task<GetCursOnDateResponse> GetCursOnDateAsync(DateTime onDate, CancellationToken cancellationToken = default)
    {
        var @namespace = (XNamespace)"http://web.cbr.ru/";
        var element = new XElement(
            @namespace + "GetCursOnDateXML",
            new XElement(@namespace + "On_date", onDate.ToString("O")));

        var uri = new Uri("http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx");
        var response = await _client.PostAsync(
            uri,
            SoapVersion.Soap12,
            [element],
            action: "http://web.cbr.ru/GetCursOnDateXML",
            cancellationToken: cancellationToken);

        var body = await response.ReadSoapXmlBodyAsync(cancellationToken);
        var descendant = body.Descendants(@namespace + "GetCursOnDateXMLResult").First();
        var result = new XDocument(descendant.Elements());

        var serializer = new XmlSerializer(typeof(GetCursOnDateResponse));
        return (GetCursOnDateResponse)serializer.Deserialize(result.CreateReader())!;
    }
}
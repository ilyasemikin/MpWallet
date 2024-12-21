using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MpWallet.SoapClient.IntegrationTests;

public class RawSoapClientTests
{
    [Theory]
    [InlineData(SoapVersion.Soap11)]
    [InlineData(SoapVersion.Soap12)]
    public async Task PostAsync_ShouldSuccess(SoapVersion version)
    {
        var client = new RawSoapClient();

        var uri = new Uri("http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso");
        var expectedMediaType = version.ToMediaType();
        
        XNamespace @namespace = "http://www.oorsprong.org/websamples.countryinfo";
        var element = new XElement(@namespace + "ListOfContinentsByName");

        var response = await client.PostAsync(uri, version, [element]);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedMediaType, response.Content.Headers.ContentType?.MediaType);
    }
}

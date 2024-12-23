using MpWallet.CBR.Client.Models;
using MpWallet.SoapClient;
using MpWallet.SoapClient.Extensions;

namespace MpWallet.CBR.Client;

public sealed class CbrClient
{
    private const string BaseUrl = "http://www.cbr.ru/DailyInfoWebServ/";
    
    private readonly SoapClient.SoapClient _client;

    public CbrClient()
    {
        _client = new SoapClient.SoapClient();
    }

    public async Task<SoapResponse<GetCursOnDateResponse>> GetCursOnDateAsync(
        GetCursOnDateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _client.SendAsync<GetCursOnDateRequest, GetCursOnDateResponse>(
            BaseUrl + "DailyInfo.asmx",
            SoapVersion.Soap12,
            request,
            "http://web.cbr.ru/GetCursOnDateXML",
            cancellationToken);
    }
}
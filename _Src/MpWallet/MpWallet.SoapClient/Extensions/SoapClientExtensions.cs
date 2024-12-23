namespace MpWallet.SoapClient.Extensions;

public static class SoapClientExtensions
{
    public static async Task<SoapResponse<TResponseBody>> SendAsync<TRequestBody, TResponseBody>(
        this SoapClient client,
        string uriString,
        SoapVersion version,
        TRequestBody body,
        string? action = null,
        CancellationToken cancellationToken = default)
        where TRequestBody : class
        where TResponseBody : class
    {
        var uri = new Uri(uriString);
        return await client.SendAsync<TRequestBody, TResponseBody>(uri, version, body, action, cancellationToken);
    }
}
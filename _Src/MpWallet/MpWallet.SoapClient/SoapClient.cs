namespace MpWallet.SoapClient;

public sealed class SoapClient
{
    private readonly RawSoapClient _client;

    public SoapClient()
    {
        _client = new RawSoapClient();
    }

    public async Task<SoapResponse<TResponseBody>> SendAsync<TRequestBody, TResponseBody>(
        Uri uri,
        SoapVersion version,
        TRequestBody body,
        string? action = null,
        CancellationToken cancellationToken = default)
        where TRequestBody : class
        where TResponseBody : class
    {
        var element = SoapBody.ToXElement(body);

        var message = await _client.PostAsync(
            uri, 
            version, 
            [element], 
            action: action,
            cancellationToken: cancellationToken);

        return await SoapResponse<TResponseBody>.CreateAsync(message, cancellationToken);
    }
}
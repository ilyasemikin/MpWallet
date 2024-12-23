using System.Net;

namespace MpWallet.CBR.Client.IntegrationTests;

public class CbrClientTests
{
    [Fact]
    public async Task GetCursOnDateAsync_ShouldSuccess()
    {
        var now = DateTime.UtcNow.Date;

        var client = new CbrClient();

        var response = await client.GetCursOnDateAsync(now);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = response.TryGetBody(out var body);
        Assert.True(result);
        Assert.NotNull(body);
        Assert.NotEmpty(body.Result.Data.OnDate);
        Assert.NotEmpty(body.Result.Data.Valutes);
    }
}

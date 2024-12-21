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
        Assert.NotEmpty(response.OnDate);
        Assert.NotEmpty(response.Valutes);
    }
}

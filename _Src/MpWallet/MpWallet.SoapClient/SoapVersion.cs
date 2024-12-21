using System.Net.Mime;
using System.Xml.Linq;

namespace MpWallet.SoapClient;

public enum SoapVersion
{
    Soap11,
    Soap12
}

internal static class SoapVersionExtensions
{
    public static string ToMediaType(this SoapVersion version)
    {
        return version switch
        {
            SoapVersion.Soap11 => MediaTypeNames.Text.Xml,
            SoapVersion.Soap12 => MediaTypeNames.Application.Soap,
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, null)
        };
    }

    public static XNamespace ToXmlNamespace(this SoapVersion version)
    {
        return version switch
        {
            SoapVersion.Soap11 => "http://schemas.xmlsoap.org/soap/envelope/",
            SoapVersion.Soap12 => "http://www.w3.org/2003/05/soap-envelope",
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, null)
        };
    }

    public static string ToSoapElement(this SoapVersion version)
    {
        return version switch
        {
            SoapVersion.Soap11 => "soap",
            SoapVersion.Soap12 => "soap12",
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, null)
        };
    }
}
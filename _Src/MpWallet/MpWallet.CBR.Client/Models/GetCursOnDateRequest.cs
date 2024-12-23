using System.Xml.Serialization;

namespace MpWallet.CBR.Client.Models;

[XmlRoot(ElementName = "GetCursOnDateXML", Namespace = "http://web.cbr.ru/")]
public sealed class GetCursOnDateRequest
{
    [XmlIgnore]
    public DateTime DateTime { get; }
    
    [XmlElement("On_date")]
    public string DateTimeString { get; }
    
    public GetCursOnDateRequest(DateTime dateTime)
    {
        DateTime = dateTime;
        DateTimeString = dateTime.ToString("O");
    }

    public static implicit operator GetCursOnDateRequest(DateTime dateTime)
    {
        return new GetCursOnDateRequest(dateTime);
    }
    
    public static implicit operator GetCursOnDateRequest(DateTimeOffset dateTime)
    {
        return new GetCursOnDateRequest(dateTime.UtcDateTime);
    }
}
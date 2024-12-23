using System.Xml.Serialization;

namespace MpWallet.CBR.Client.Models;

[XmlRoot("GetCursOnDateXMLResponse", Namespace = "http://web.cbr.ru/")]
public sealed class GetCursOnDateResponse
{
    [XmlElement("GetCursOnDateXMLResult")]
    public required GetCursOnDateResult Result { get; init; }

    public sealed class GetCursOnDateResult
    {
        [XmlElement("ValuteData", Namespace = "")]
        public required ValuteData Data { get; init; }
    }
    
    public sealed class ValuteData
    {
        [XmlElement("ValuteCursOnDate")]
        public required ValuteCursOnDate[] Valutes { get; init; }
    
        [XmlAttribute("OnDate")]
        public required string OnDate { get; init; }
    }
    
    public sealed class ValuteCursOnDate
    {
        [XmlElement("Vname")]
        public required string Name { get; init; }
        
        [XmlElement("Vnom")]
        public required int Nom { get; init; }
        
        [XmlElement("Vcurs")]
        public required decimal Curs { get; init; }
        
        [XmlElement("Vcode")]
        public required string Code { get; init; }
        
        [XmlElement("VchCode")]
        public required string ChCode { get; init; }
        
        [XmlElement("VunitRate")]
        public required decimal UnitRate { get; init; }
    }
}
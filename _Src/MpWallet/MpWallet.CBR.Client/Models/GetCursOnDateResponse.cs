using System.Xml.Serialization;

namespace MpWallet.CBR.Client.Models;

[XmlRoot("ValuteData")]
public class GetCursOnDateResponse
{
    [XmlElement("ValuteCursOnDate")]
    public required ValuteCursOnDate[] Valutes { get; init; }
    
    [XmlAttribute("OnDate")]
    public required string OnDate { get; init; }
    
    public class ValuteCursOnDate
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
        public required string UnitRate { get; init; }
    }
}
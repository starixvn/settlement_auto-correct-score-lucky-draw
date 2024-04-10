using System.Xml.Serialization;

namespace CorrectScoreLuckyDraw.Models;

public class Member
{
    [XmlAttribute("t")]
    public long TransId { get; set; }

    [XmlAttribute("c")]
    public int CustId { get; set; }

    [XmlAttribute("s")]
    public int StatusId { get; set; }
}
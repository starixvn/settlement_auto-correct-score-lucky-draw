using System.Xml.Serialization;

namespace CorrectScoreLuckyDraw.Models;

[XmlRoot("root")]
public class LuckyDrawMember
{
    [XmlElement("r")]
    public List<Member> Members { get; set; }
}
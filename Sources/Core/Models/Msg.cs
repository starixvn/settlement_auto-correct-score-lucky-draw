using System.Xml.Serialization;

namespace Core.Models;

[XmlRoot("Messages")]
public class Msg
{
	[XmlElement("Item")]
	public List<MessageItem> Items { get; set; }
}

public class MessageItem
{
	[XmlAttribute]
	public int TargetId { get; set; }

	[XmlAttribute]
	public string TargetName { get; set; }

	[XmlAttribute]
	public string BetTransId { get; set; } = "Null";
}
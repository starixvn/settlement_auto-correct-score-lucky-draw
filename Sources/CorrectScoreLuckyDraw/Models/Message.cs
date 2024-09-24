using System.Xml.Serialization;

namespace CorrectScoreLuckyDraw.Models;

[XmlRoot("Messages")]
public class Message
{
	[XmlElement("Item")]
	public List<MessageItem> Items { get; set; }
}

public class MessageItem
{
	[XmlAttribute]
	public int MessageId { get; set; }

	[XmlAttribute]
	public int TargetId { get; set; }

	[XmlAttribute]
	public string TargetName { get; set; }

	[XmlAttribute]
	public string BetTransId { get; set; } = "Null";
}
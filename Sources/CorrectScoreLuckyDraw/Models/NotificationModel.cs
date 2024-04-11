namespace CorrectScoreLuckyDraw.Models;

public class NotificationModel
{
    public IEnumerable<Ticket> Tickets { get; set; }

    public Match Match { get; set; }
}
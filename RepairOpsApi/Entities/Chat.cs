public class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid(); //chat id
    public Guid CaseId { get; set; } //reference til sag
    public string Sender { get; set; } //Kunde eller tekniker
    public string Message { get; set; } //Beskeden
    public DateTime Timestamp { get; set; } = DateTime.Now; //Hvornår besked bliver sendt

    //Relation til Case
    public Case Case { get; set; }
}

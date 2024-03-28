namespace GoogleCalendarWebApp.Models;

public class Event
{
    public Event()
    {
        this.Start = new EventDateTime()
        {
            TimeZone = "Asia/Tehran"
        };

        this.End = new EventDateTime()
        {
            TimeZone = "Asia/Tehran"
        };
    }

    public string Summry { get; set; }
    public string Description { get; set; }
    public EventDateTime Start { get; set; }
    public EventDateTime End { get; set; }

}

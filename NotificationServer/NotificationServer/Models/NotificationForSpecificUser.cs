namespace NotificationServer.Models
{
    public class NotificationForSpecificUser
    {
        public Notification Notification { get; set; }
        public string ClientId { get; set; }
    }
}
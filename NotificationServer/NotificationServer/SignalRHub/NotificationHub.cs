using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace NotificationServer.SignalRHub
{
    [HubName("StatusHub")]
    public class NotificationHub : Hub
    {
    }
}
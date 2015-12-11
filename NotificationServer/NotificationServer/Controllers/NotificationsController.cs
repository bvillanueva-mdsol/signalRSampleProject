using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using NotificationServer.Models;
using NotificationServer.SignalRHub;

namespace NotificationServer.Controllers
{
    public class NotificationsController : ApiController
    {
        private readonly IHubContext statusHub;

        public NotificationsController()
        {
            statusHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }

        // 1. broadcast to all
        [Route("api/broadcast")]
        [HttpPost]
        public HttpResponseMessage Broadcast([FromBody] Notification notification)
        {
            statusHub.Clients.All.updateStatus(notification);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // 2. send to particular client from all
        [Route("api/send")]
        [HttpPost]
        public HttpResponseMessage Send([FromBody] NotificationForSpecificUser notificationForSpecificUser)
        {
            var targetClient = statusHub.Clients.Client(notificationForSpecificUser.ClientId);
            if (targetClient == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            statusHub.Clients.Client(notificationForSpecificUser.ClientId).updateStatus(notificationForSpecificUser.Notification);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

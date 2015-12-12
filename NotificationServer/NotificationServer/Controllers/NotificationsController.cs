using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        // register to a group
        [Route("api/group/register")]
        [HttpPost]
        public async Task<HttpResponseMessage> Register([FromBody] UserGroupRegistration userRegistration)
        {
            await statusHub.Groups.Add(userRegistration.ClientId, userRegistration.GroupName);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // send to particular client
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

        // broadcast to a group
        [Route("api/group/broadcast")]
        [HttpPost]
        public HttpResponseMessage GroupBroadcast([FromBody] NotificationForGroup notificationForGroup)
        {
            statusHub.Clients.Group(notificationForGroup.GroupName).updateStatus(notificationForGroup.Notification);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // broadcast to all
        [Route("api/broadcast")]
        [HttpPost]
        public HttpResponseMessage Broadcast([FromBody] Notification notification)
        {
            statusHub.Clients.All.updateStatus(notification);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

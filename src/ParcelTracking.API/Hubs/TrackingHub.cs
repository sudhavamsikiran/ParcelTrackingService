using Microsoft.AspNetCore.SignalR;
namespace ParcelTracking.API.Hubs
{
  public class TrackingHub : Hub
    {
        public async Task SubscribeTracking(string trackingId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, trackingId);
        }
    }
}

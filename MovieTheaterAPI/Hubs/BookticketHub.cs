using Microsoft.AspNetCore.SignalR;

namespace MovieTheaterAPI.Hubs
{
    public class BookticketHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessageConnect", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
    }
}

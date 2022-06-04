using Microsoft.AspNetCore.SignalR;

namespace ChartServer.Hubs
{
    public class SatisHub:Hub
    {
        public async Task SendMessageAsync()
        {
           await Clients.All.SendAsync("receiveMsg", "Merhaba");
        }
    }
}

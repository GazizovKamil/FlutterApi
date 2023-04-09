using FlutterApi.Data;
using Microsoft.AspNetCore.SignalR;

namespace FlutterApi.Hubs
{
    public class SignalrHub : Hub
    {
        public void BroadcastUser(User user)
        {
            Clients.All.SendAsync("User", user);
        }

        public void BroadcastMessage(string msq)
        {
            Clients.All.SendAsync("Message", msq);
        }
    }
}

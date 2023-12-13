using Microsoft.AspNetCore.SignalR;

namespace SignalR.Blazor
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> _users = new Dictionary<string, string>();

        public async Task Start(string user, string message)
        {
            _users.Add(Context.ConnectionId, message);

            await Clients.Others.SendAsync("Other", user);
            await Clients.Caller.SendAsync("Welcome", user);    
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

using Microsoft.AspNetCore.SignalR;

namespace SignalR.Blazor
{
    public class ChatHub : Hub
    {
        private static List<UserModel> messages = new List<UserModel>();

        private static Dictionary<string, string> _users = new Dictionary<string, string>();

        public async Task Start(string user, string message)
        {
            _users.Add(Context.ConnectionId, message);

            foreach (var i in messages)
            {
                await Clients.Caller.SendAsync("History", i);
            }

            await Clients.Others.SendAsync("Other", user);

            await Clients.Caller.SendAsync("Welcome", user);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            messages.Add(new UserModel() { Message = message, UserName = user });
        }

        public async Task SendToAllMessage(string user, string message)
            => await Clients.All.SendAsync("ReceiveMessage", user, Context.ConnectionId, message);

        public async Task SendToUserMessage(string id, string user, string message)
        {
            await Clients.Client(id).SendAsync("ReceiveMessageFromUser", user, message);
            await Clients.Caller.SendAsync("ReceiveMessageFromUser", user, message);

        }

        public async Task SendToGroupMessage(string groupName, string user, string message)
            => await Clients.Group(groupName).SendAsync("ReceiveMessageFromGroup", user, message);

        public async Task JoinGroup(string groupName)
            => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
}
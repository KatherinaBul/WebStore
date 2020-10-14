using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebStore.Hubs
{
    public class InformationHub : Hub
    {
        public async Task SendMessage(string message) => await Clients.All.SendAsync("MessageFromClient", message);
    }
}
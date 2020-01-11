using DataAccess;
using Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;

namespace WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHubContext<ChatHub> _context;

        public ChatHub(IChatRepository chatRepository, IHubContext<ChatHub> context)
        {
            _chatRepository = chatRepository;
            _context = context;
        }

        public async void SendToAll(string name, string message)
        {
            var m = new ChatMessage(name, message, DateTime.Now);
            await Clients.All.SendAsync("sendToAll", m);
            await _chatRepository.ArchiveChat(m);
        }

        public async void InitChat()
        {
            var id = Context.ConnectionId;
            var messages = (await _chatRepository.GetLast(10)).OrderBy(m => m.DateTime);
            await _context.Clients.Client(id).SendAsync("messageQueue", messages);
        }
    }
}

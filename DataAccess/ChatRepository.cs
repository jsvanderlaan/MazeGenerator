using Entities;
using Raven.Client.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ChatRepository : BaseRepository, IChatRepository
    {
        public ChatRepository() : base("Chat")
        {

        }

        public async Task ArchiveChat(ChatMessage chat) => await Store(chat);

        public async Task<List<ChatMessage>> GetLast(int count) => await Get(session => 
            session.Query<ChatMessage>()
                .OrderByDescending<ChatMessage>(c => c.DateTime)
                .Take(count)
                .ToListAsync());
    }
}

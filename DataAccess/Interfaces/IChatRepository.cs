using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IChatRepository
    {
        Task ArchiveChat(ChatMessage chat);
        Task<List<ChatMessage>> GetLast(int count);
    }
}
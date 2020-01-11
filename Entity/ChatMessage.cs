using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ChatMessage
    {
        public ChatMessage(string user, string message, DateTime dateTime)
        {
            User = user;
            Message = message;
            DateTime = dateTime;
        }
        public string User { get; }
        public string Message { get; }
        public DateTime DateTime { get; }
    }
}

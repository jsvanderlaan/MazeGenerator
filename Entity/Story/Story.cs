using System;

namespace Entities.Story
{
    public class Story
    {
        public Story(string author, string title, string message, DateTime creationTime)
        {
            Author = author;
            Title = title;
            Message = message;
            CreationTime = creationTime;
        }

        public string Author { get; }
        public string Title { get; }
        public string Message { get; }
        public DateTime CreationTime { get; }
    }
}

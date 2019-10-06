using System;

namespace Entities.Counter
{
    public class ClickCount : ICount
    {
        public ClickCount(string name)
        {
            Name = name;
            CreationDateTime = DateTime.Now;
        }
        public string Name { get; }
        public DateTime CreationDateTime { get; }
    }
}

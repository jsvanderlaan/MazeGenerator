using System;

namespace Entities.Counter
{
    public interface ICount
    {
        string Name { get; }
        DateTime CreationDateTime { get; }
    }
}

using Common;
using DataTransferObjects;
using System.Collections.Generic;
using Raven.Client.Documents.Session;
using System.Threading.Tasks;
using System;

namespace DataAccess
{
    public interface IBaseRepository
    {
        Task Store(object obj);
        Task Store(MazeDto maze, List<ImageDto> images, List<Timer> timers);
        Task<List<T>> Get<T>(Func<IAsyncDocumentSession, Task<List<T>>> func);
    }
}
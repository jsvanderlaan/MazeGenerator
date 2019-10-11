﻿using DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IBaseRepository
    {
        Task Store(object obj);
        Task Store(object obj, List<ImageDto> images);
    }
}
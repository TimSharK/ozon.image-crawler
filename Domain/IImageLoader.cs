using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    public interface IImageLoader
    {
        Task Load();
    }
}

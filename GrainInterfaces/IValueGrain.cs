using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IValueGrain : IGrainWithIntegerKey
    {
        Task<string> GetValue();
        Task<int> CompareFace(string faceInfo);

        Task SetValue(string value);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace PP9.GrainInterfaces
{
    public interface IDataGenerator : IGrainWithIntegerKey
    {
        Task<int[]> GenerateVector(int size);
        Task<int[,]> GenerateMatrix(int size);
    }
}

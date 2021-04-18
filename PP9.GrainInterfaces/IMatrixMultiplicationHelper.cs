using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace PP9.GrainInterfaces
{
    public interface IMatrixMultiplicationHelper : IGrainWithIntegerKey
    {
        Task<int[]> Multiply(int[,] matrix, int[] vector);
    }
}

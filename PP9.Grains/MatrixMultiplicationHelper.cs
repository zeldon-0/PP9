using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using PP9.GrainInterfaces;

namespace PP9.Grains
{
    public class MatrixMultiplicationHelper : Grain, IMatrixMultiplicationHelper
    {
        private readonly ILogger _logger;
        public MatrixMultiplicationHelper(ILogger<DataGenerator> logger)
        {
            _logger = logger;
        }
        public Task<int[]> Multiply(int[,] matrix, int[] vector)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = new int[matrix.GetLength(0)];
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                var matrixRow =
                    Enumerable.Range(0, matrix.GetLength(1))
                        .Select(x => matrix[i, x])
                        .ToArray();
                result[i] = Multiply(matrixRow, vector);
            }
            stopwatch.Stop();
            _logger.LogInformation($"\n Multiplication complete in {stopwatch.ElapsedMilliseconds}ms");

            return Task.FromResult(result);
        }
        private int Multiply(int[] matrixRow, int[] vector) =>
            Enumerable.Range(0, vector.Length)
                .Select(i => matrixRow[i] * vector[i])
                .Sum();
    }
}

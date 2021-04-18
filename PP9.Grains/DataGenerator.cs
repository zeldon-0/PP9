using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using PP9.GrainInterfaces;

namespace PP9.Grains
{
    public class DataGenerator : Grain, IDataGenerator
    {
        private readonly ILogger _logger;

        public DataGenerator(ILogger<DataGenerator> logger)
        {
            _logger = logger;
        }

        public Task<int[,]> GenerateMatrix(int size)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var random = new Random();
            var matrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    matrix[i, j] = random.Next(-100, 101);
            }
            stopwatch.Stop();
            _logger.LogInformation($"\n Matrix generated in {stopwatch.ElapsedMilliseconds}ms");

            return Task.FromResult(matrix);
        }

        public Task<int[]> GenerateVector(int size)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var random = new Random();
            var vector = new int[size];
            for (int i = 0; i < size; i++)
            {
                vector[i] = random.Next(-100, 101);
            }
            stopwatch.Stop();
            _logger.LogInformation($"\n Vector generated in {stopwatch.ElapsedMilliseconds}ms");
            return Task.FromResult(vector);
        }
    }
}

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using PP9.GrainInterfaces;

namespace PP9.Client
{
    class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            int[,] matrix;
            int[] vector;
            int[] result;
            int size = 1000;
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                using (var client = await ConnectDataClient())
                {
                    var dataService = client.GetGrain<IDataGenerator>(0);
                    matrix = await dataService.GenerateMatrix(size);
                    vector = await dataService.GenerateVector(size);
                }

                using (var client = await ConnectMultiplicationClient())
                {
                    var multiplicationService = client.GetGrain<IMatrixMultiplicationHelper>(0);
                    result = await multiplicationService.Multiply(matrix, vector);
                }
                stopwatch.Stop();
                Console.WriteLine($"\n Calculations complete in {stopwatch.ElapsedMilliseconds} ms.");

                Console.ReadKey();

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectDataClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering(30000)
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static async Task<IClusterClient> ConnectMultiplicationClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering(30001)
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }
    }
}

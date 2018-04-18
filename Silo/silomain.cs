using Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using GrainInterfaces;
using System.Runtime.InteropServices;

namespace Silo
{
    public class Program
    {
         // const string sofile="libcsdk-face.so";
           const string sofile="core_sdk.dll";
        [DllImport(sofile, CallingConvention = CallingConvention.Cdecl)]
        public extern static int mgv_set_log(int level);
        [DllImport(sofile, CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int mgv_create_engine(string model_path, Engine** pengine);
        [DllImport(sofile, CallingConvention = CallingConvention.Cdecl)]
        public extern static string mgv_get_error_str(int code);
        [DllImport(sofile, CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int mgv_destroy_engine(Engine* engine);
        private static ISiloHost silo;
        private static readonly ManualResetEvent siloStopped = new ManualResetEvent(false);
        
        static void Main(string[] args)
        {
            // TODO replace with your connection string
           // const string connectionString = "YOUR_CONNECTION_STRING_HERE";
           mgv_set_log(0);
          unsafe{
              Engine* engine=null; mgv_create_engine("",&engine);
           silo = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "orleansdockerwufei";
                    options.ServiceId = "NoThief";
                }).UseLocalhostClustering()
              //  .UseAzureStorageClustering(options => options.ConnectionString = connectionString)
               // .ConfigureEndpoints(siloPort: 12345, gatewayPort: 34567)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ValueGrain).Assembly).WithReferences())
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.Warning).AddConsole())
                .Build();

            Task.Run(StartSilo);

            AssemblyLoadContext.Default.Unloading += context =>
            {
                Task.Run(StopSilo);
                siloStopped.WaitOne();
            };

            siloStopped.WaitOne();
            mgv_destroy_engine(engine);
          }
           
        }

        private static async Task StartSilo()
        {
            await silo.StartAsync();
            Console.WriteLine("Silo started");
        }

        private static async Task StopSilo()
        {
            await silo.StopAsync();
            Console.WriteLine("Silo stopped");
            siloStopped.Set();
        }
    }
}

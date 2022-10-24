using Microsoft.Extensions.DependencyInjection;
using System;

namespace NFTValuations
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = Startup.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            _ = serviceProvider.GetService<EntryPoint>().RunAsync(args);

            Console.ReadLine();
        }
    }
}
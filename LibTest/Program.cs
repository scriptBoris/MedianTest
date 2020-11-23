using System;
using System.Threading.Tasks;

namespace LibTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var worker = new Worker.Worker("Data.txt");

            int number = 0;
            await foreach(float res in worker.ReadFile())
            {
                Console.WriteLine($"{number++}: {res}");
            }

            Console.WriteLine("Hello World!");
        }
    }
}

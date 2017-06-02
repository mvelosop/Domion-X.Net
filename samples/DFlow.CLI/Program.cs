using System;
using System.Text;

namespace Demo.Cli
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("EF Core CLI host application");

            Console.ReadKey();
        }
    }
}

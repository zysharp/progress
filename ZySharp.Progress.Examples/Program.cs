using System;

namespace ZySharp.Progress.Examples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExampleMultiStepProgress.Run();
            Console.WriteLine();
            ExampleMultiStepProgress.RunEnum();
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using BindingFlags = System.Reflection.BindingFlags;

namespace TestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var methods = typeof(Task).GetMethods(BindingFlags.Static|BindingFlags.InvokeMethod|BindingFlags.Public);
            Task.Run(async () => await Task.CompletedTask);
            foreach (var method in methods.Where(info => info.Name == "Run"))
            {
                var pList =string.Join(", ", method.GetParameters().Select(p => p.ParameterType.FullName));
                Console.WriteLine($"Run({pList})");
            }
            
            Console.ReadKey();
        }
    }
}

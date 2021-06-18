using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Threading.Tasks;


namespace TF1
{

    interface I1
    {
        void PrintSomething();
    }

    interface I2
    {
        void PrintSomethingElse();
    }


    class X1 : I1
    {
        public void PrintSomething()
        {
            Console.WriteLine("Something");
        }
    }

    class X2 : I2
    {
        public void PrintSomethingElse()
        {
            Console.WriteLine("Something else!");
        }
    }


    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //var f = new TypeFactory<I1, MarkerAttribute>();
                //var x = f.CreateInstance(nameof(X1));
                //x.PrintSomething();


                var f1 = new TypeFactory2<I1>();
                var f2 = new TypeFactory2<I2>();

                var x1 = f1.CreateInstance(nameof(X1));
                x1.PrintSomething();
                var x2 = f2.CreateInstance(nameof(X2));
                x2.PrintSomethingElse();
            }
            catch (Exception ex)
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var progname = Path.GetFileNameWithoutExtension(fullname);
                Console.Error.WriteLine($"{progname} Error: {ex.Message}");
            }

        }
    }
}

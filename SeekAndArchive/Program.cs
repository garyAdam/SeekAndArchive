using System;
using System.Collections.Generic;

namespace SeekAndArchive
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            FileSearcher fileSearcher = new FileSearcher();
            IEnumerable<string> result = fileSearcher.Search(args[0], args[1]);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }


        }


    }
}

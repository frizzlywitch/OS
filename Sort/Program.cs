using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OS2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var sortList = new List<long>();
                foreach (string fileName in args)
                {
                    if (!File.Exists(fileName))
                    {
                        Console.WriteLine("Error(1): no such file '{0}'. This file name has ignored.", fileName);
                        continue;
                    }

                    string text = File.ReadAllText(fileName);

                    if (string.IsNullOrEmpty(text))
                    {
                        Console.WriteLine("Warning(1): File '{0}' is empty.", fileName);
                        continue;
                    }

                    foreach (string substr in text.Split(' ', '\n', '\r', '\t').Where(s => !string.IsNullOrEmpty(s)))
                    {
                        try
                        {
                            var num = long.Parse(substr);
                            sortList.Add(num);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine(
                                "Error(2): not a number '{0}' in file '{1}'. This number is ignored.", substr,
                                fileName);
                        }
                    }
                }

                sortList.Sort();
                foreach (var num in sortList)
                {
                    Console.Write(num +" ");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Unknown error: {0}",e.Message);
            }
        }
    }
}
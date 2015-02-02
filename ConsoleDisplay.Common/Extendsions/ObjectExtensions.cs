﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDisplay
{
    public static class ObjectExtensions
    {
        public static void ToConsole(this object obj, string prefix= "", string suffix = "")
        {
            Console.WriteLine("{1}{0}{2}", obj ?? "null", prefix, suffix);
        }
    }
}

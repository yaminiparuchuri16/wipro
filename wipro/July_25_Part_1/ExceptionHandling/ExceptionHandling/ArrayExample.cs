﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandling
{
    internal class ArrayExample
    {
        static void Main()
        {
            int[] a = new int[] { 5, 8 };

            try
            {
                a[10] = 323;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Array Size is Small...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

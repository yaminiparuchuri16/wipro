﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IspWith
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Murali murali = new Murali();
            murali.ShowPersonalInfo();
            Rajesh rajesh = new Rajesh();
            rajesh.ShowPersonalInfo();
            rajesh.ProjectName();
            rajesh.BillingInfo();
        }
    }
}

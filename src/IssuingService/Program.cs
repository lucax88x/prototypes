using System;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace IssuingService
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://*:9000/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("App started with address '{0}' on a machine '{1}'", baseAddress, Environment.OSVersion);
                Console.WriteLine();
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}

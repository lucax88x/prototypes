using System;
using System.Net.Http;
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
                Console.WriteLine("App started with address: {0}", baseAddress);
                Console.WriteLine();
                Console.WriteLine();

                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync("http://localhost:9000/api/cardholder/asdf").Result;
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Console.WriteLine();
                Console.WriteLine();

                response = client.GetAsync("http://localhost:9000/api/cardholders").Result;
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}

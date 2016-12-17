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
            string baseAddress = "http://localhost:9000/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/cardholder/asdf").Result;
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Console.WriteLine();
                Console.WriteLine();

                response = client.GetAsync(baseAddress + "api/cardholders").Result;
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}

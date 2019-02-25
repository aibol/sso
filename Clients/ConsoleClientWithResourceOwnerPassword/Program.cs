﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ConsoleClientWithResourceOwnerPassword
{
    class Program
    {
        static void Main(string[] args)
        {
            TestResourceOwnerPassword().GetAwaiter();

            Console.ReadLine();
        }

        private static async Task TestResourceOwnerPassword()
        {
            var disco = await DiscoveryClient.GetAsync("https://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "test.rop", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("test@aibol.com", "Abc123!", "test.api");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }
    }
}

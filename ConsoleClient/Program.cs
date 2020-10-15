using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;

namespace ConsoleClient
{
    class Program
    {
        private static async Task Main()
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = disco.TokenEndpoint,
            //    ClientId = "oauthClient",
            //    ClientSecret = "SuperSecretPassword",

            //    Scope = "api1"
            //});

            //if (tokenResponse.IsError)
            //{
            //    Console.WriteLine(tokenResponse.Error);
            //   // return;
            //}

            //Console.WriteLine(tokenResponse.Json);
            //Console.WriteLine("\n\n");

            //// call api
            //var apiClient = new HttpClient();
            //apiClient.SetBearerToken(tokenResponse.AccessToken);

            //var response = await apiClient.GetAsync("https://localhost:6001/identity");
            //if (!response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(response.StatusCode);
            //}
            //else
            //{
            //    var content = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine(JArray.Parse(content));
            //}

            // 

            
            PasswordTokenRequest tokenRequest = new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = "oidcClient",
                ClientSecret = "SuperSecretPassword",
                UserName = "abhishek.sarolia@anacapfp.com",
                Password = "Password@123",
            };

            var response =  await client.RequestPasswordTokenAsync(tokenRequest);
            if (!response.IsError)
            {
                Console.WriteLine(response.AccessToken);
            }
            else
            {
                throw new Exception("Invalid username or password");
            }

            var temptoken = response.AccessToken;
            //var client = new HttpClient();

            //response = await client.GetUserInfoAsync(new UserInfoRequest
            //{
            //    Address = disco.UserInfoEndpoint,
            //    Token = temptoken
            //});

            var userInfoRequest = new UserInfoRequest()
            {
                Address = disco.UserInfoEndpoint,
                Token = temptoken
            };

            UserInfoResponse resp = await client.GetUserInfoAsync(userInfoRequest);



        }
    }
}

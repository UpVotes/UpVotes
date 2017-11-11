using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UpVotes.Utility
{
    public class Utility
    {
        public static async System.Threading.Tasks.Task<string> GetAuthToken(string userName, string password)
        {
            string baseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString();
            string authToken = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:18515");
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/GetAuthToken");

                var byteArray = new System.Text.UTF8Encoding().GetBytes(userName + ":" + password);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    authToken = Convert.ToString(response.Headers.Where(i => i.Key == "Token").Select(i => i.Value).ToList()[0].FirstOrDefault());
                }
            }

            return authToken;
        }
    }
}
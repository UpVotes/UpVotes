using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UpVotes.Utility
{
    public class Utility
    {
        public static async System.Threading.Tasks.Task<string> GetAuthToken(string userName, string password)
        {
            string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
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

        public static string ExtractText(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var chunks = new List<string>();

            foreach (var item in doc.DocumentNode.DescendantsAndSelf())
            {
                if (item.NodeType == HtmlNodeType.Text)
                {
                    if (item.InnerText.Trim() != "")
                    {
                        chunks.Add(item.InnerText.Trim());
                    }
                }
            }
            return String.Join(" ", chunks);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace UpVotes.Utility
{
    public static class ClientHelper
    {
        public static HttpClient GetClient(string username, string password)
        {
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(username +":" + password)));

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue },
                BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString())
            //Set some other client defaults like timeout / BaseAddress
        };
            return client;
        }
    }
}
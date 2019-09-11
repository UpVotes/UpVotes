using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;
using System;

namespace UpVotes.Business
{
    public class ContactUsService
    {
        HttpClient _httpClient = null;        

        internal int SaveContactInfo(ContactUsInfoEntity contactEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveContactUs";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(contactEntity), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return 0;
                }
            }
        }

        internal int SaveSponsorInfo(SponsorerInfoEntity sponsorEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveSponsorerInfo";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(sponsorEntity), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
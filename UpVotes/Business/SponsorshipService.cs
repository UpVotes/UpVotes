using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;

namespace UpVotes.Business
{
    public class SponsorshipService
    {
        private HttpClient _httpClient = null;        

        
        internal bool ApplySponsorship(SponsorshipRequestEntity sponsorshipReqObj)
        {           
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "ApplySponsorShip";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(sponsorshipReqObj), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                bool isApplied = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return isApplied;
            }
        }

        internal List<SponsorshipExpiredListEntity> GetExpiredSponsorshipList(SponsorshipRequestEntity sponsorshipReqObj)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetAllExpiredSponsorshipList";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(sponsorshipReqObj), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                List<SponsorshipExpiredListEntity> expiredListObj = new List<SponsorshipExpiredListEntity>();
                if (response.IsSuccessStatusCode)
                {
                    expiredListObj = JsonConvert.DeserializeObject<List<SponsorshipExpiredListEntity>>(response.Content.ReadAsStringAsync().Result);
                    return expiredListObj;
                }
                else
                {
                    return expiredListObj;
                }

            }
        }

        internal bool SchedulerToDeactivateSponsor(SponsorshipRequestEntity sponsorshipReqObj)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SchedulerToDeactivateSponsor";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(sponsorshipReqObj), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                bool isDeactivated = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                return isDeactivated;
            }
        }
    }
}
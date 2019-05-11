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
    public class OverviewAndNewsService
    {
        HttpClient _httpClient = null;

        internal OverviewNewsViewModel GetCompanySoftwareNews(OverviewNewsEntity newsFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetSoftwareCompanyNews";
                string completeURL = WebAPIURL + apiMethod;

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(newsFilter), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                OverviewNewsViewModel newsViewModel = new OverviewNewsViewModel();
                if (response.IsSuccessStatusCode)
                {
                    newsViewModel = JsonConvert.DeserializeObject<OverviewNewsViewModel>(response.Content.ReadAsStringAsync().Result);
                    
                    return newsViewModel;
                   
                }
                else
                {
                    return newsViewModel;
                }
            }
        }

        internal OverviewNewsViewModel GetCompanySoftwareNewsByName(OverviewNewsEntity newsFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetSoftwareCompanyNewsByName";
                string completeURL = WebAPIURL + apiMethod;

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(newsFilter), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                OverviewNewsViewModel newsViewModel = new OverviewNewsViewModel();
                if (response.IsSuccessStatusCode)
                {
                    newsViewModel = JsonConvert.DeserializeObject<OverviewNewsViewModel>(response.Content.ReadAsStringAsync().Result);

                    return newsViewModel;

                }
                else
                {
                    return newsViewModel;
                }
            }
        }

        internal int SaveNews(OverviewNewsEntity NewsEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveAdminNews";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(NewsEntity), Encoding.UTF8, "application/json");

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

        internal int SaveUserNews(OverviewNewsEntity NewsEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveUserNews";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(NewsEntity), Encoding.UTF8, "application/json");

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

        internal bool IsNewsExists(OverviewNewsEntity newsrequest)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "IsNewsExists";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(newsrequest), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;                
                bool isExists = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);                
                return isExists;
            }
        }

    }
}
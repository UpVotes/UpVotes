using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Business
{
    public class FocusAreaService
    {
        private string _baseURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
        private HttpClient _httpClient = null;
        private string _apiMethod = string.Empty;
        private string _completeURL = string.Empty;

        public List<FocusAreaEntity> GetFocusAreas()
        {
            using (_httpClient = new HttpClient())
            {
                this._apiMethod = "GetFocusAreas";
                this._completeURL = _baseURL + _apiMethod + '/';
                var response = _httpClient.GetStringAsync(this._completeURL).Result;
                List<FocusAreaEntity> focusAreaList = JsonConvert.DeserializeObject<List<FocusAreaEntity>>(response);

                return focusAreaList;
            }
        }

        public int GetFocusAreaID(string urlfocusAreaName)
        {
            using (_httpClient = new HttpClient())
            {
                this._apiMethod = "GetFocusAreaID";
                this._completeURL = _baseURL + _apiMethod + '/' + GetActualFocusAreaName(urlfocusAreaName);
                var response = _httpClient.GetStringAsync(this._completeURL).Result;
                int focusAreaID = JsonConvert.DeserializeObject<int>(response);
                return focusAreaID;
            }
        }

        private string GetActualFocusAreaName(string urlFocusAreaName)
        {
            string focusAreaName = string.Empty;
            switch (urlFocusAreaName.Trim())
            {
                case "mobile-application-developers":
                    focusAreaName = "Mobile App Developement";
                    break;

                case "seo-companies":
                    focusAreaName = "Search Engine Optimization";
                    break;

                case "digital-marketing-companies":
                    focusAreaName = "Digital Marketing";
                    break;

                case "web-design-companies":
                    focusAreaName = "Web Design";
                    break;

                case "software-development-companies":
                    focusAreaName = "Software Developement";
                    break;

                case "web-development-companies":
                    focusAreaName = "Web Developement";
                    break;

                default:
                    focusAreaName = "Software Developement";
                    break;
            }

            return focusAreaName;
        }
    }
}
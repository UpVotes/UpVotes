using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Business
{
    public class FocusAreaService
    {
        private string _baseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].ToString();
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

        public int GetFocusAreaID(string focusAreaName)
        {
            using (_httpClient = new HttpClient())
            {
                this._apiMethod = "GetFocusAreaID";
                this._completeURL = _baseURL + _apiMethod + '/' + focusAreaName;
                var response = _httpClient.GetStringAsync(this._completeURL).Result;
                int focusAreaID = JsonConvert.DeserializeObject<int>(response);
                return focusAreaID;
            }
        }
    }
}
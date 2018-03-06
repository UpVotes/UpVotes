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
            return GetFocusAreaIDByEnum(urlfocusAreaName);
        }

        private int GetFocusAreaIDByEnum(string urlfocusAreaName)
        {
            int focusAreaID = 0;
            switch (urlfocusAreaName.Trim())
            {
                case "mobile-application-developers":
                    focusAreaID = (int)FocusAreas.MobileAppDevelopement;
                    break;

                case "seo-companies":
                    focusAreaID = (int)FocusAreas.SearchEngineOptimization;
                    break;

                case "digital-marketing-companies":
                    focusAreaID = (int)FocusAreas.DigitalMarketing;
                    break;

                case "web-design-companies":
                    focusAreaID = (int)FocusAreas.WebDesign;
                    break;

                case "software-development-companies":
                    focusAreaID = (int)FocusAreas.SoftwareDevelopement;
                    break;

                case "web-development-companies":
                    focusAreaID = (int)FocusAreas.WebDevelopement;
                    break;
                case "ui-ux-agencies":
                    focusAreaID = (int)FocusAreas.UXUIDesign;
                    break;
                default:
                    focusAreaID = (int)FocusAreas.MobileAppDevelopement;
                    break;
            }

            return focusAreaID;
        }        
    }

    public enum FocusAreas
    {

        DigitalMarketing = 1,
        WebDesign,
        SoftwareDevelopement,
        IOTDevelopement,
        WebDevelopement,
        CustomSoftwareDevelopemt,
        SearchEngineOptimization,
        UXUIDesign,
        MobileAppDevelopement,
        EcommerceDevelopement,
        WearableAppDevelopement,
        ARVRDevelopment,
        SocialMediaMarketing,
        PayPerClick,
        ContentMarketing,
    }
}
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
                case "wearable-application-developers":
                    focusAreaID = (int)FocusAreas.WearableAppDevelopement;
                    break;
                case "ecommerce-developers":
                    focusAreaID = (int)FocusAreas.EcommerceDevelopement;
                    break;
                case "social-media-marketing-companies":
                    focusAreaID = (int)FocusAreas.SocialMediaMarketing;
                    break;
                case "ppc-companies":
                    focusAreaID = (int)FocusAreas.PayPerClick;
                    break;
                case "content-marketing-companies":
                    focusAreaID = (int)FocusAreas.ContentMarketing;
                    break;
                case "iot-application-developers":
                    focusAreaID = (int)FocusAreas.IOTDevelopement;
                    break;
                case "mobile-app-marketing-agencies":
                    focusAreaID = (int)FocusAreas.MobileAppMarketingAgencies;
                    break;
                case "email-marketing-agencies":
                    focusAreaID = (int)FocusAreas.EmailMarketingAgencies;
                    break;
                case "sem-agencies":
                    focusAreaID = (int)FocusAreas.SEMAgencies;
                    break;
                case "branding-agencies":
                    focusAreaID = (int)FocusAreas.BrandingAgencies;
                    break;
                case "pr-agencies":
                    focusAreaID = (int)FocusAreas.PRAgencies;
                    break;
                case "digital-strategy-agencies":
                    focusAreaID = (int)FocusAreas.DigitalStrategyAgencies;
                    break;
                case "video-production-agencies":
                    focusAreaID = (int)FocusAreas.VideoProductionAgencies;
                    break;
                case "chatbot-development":
                    focusAreaID = (int)FocusAreas.ChatbotDevelopment;
                    break;
                case "ar-vr-application-development":
                    focusAreaID = (int)FocusAreas.ARVRApplicationDevelopment;
                    break;
                case "graphic-designers":
                    focusAreaID = (int)FocusAreas.GraphicDesigners;
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
        ARVRApplicationDevelopment,
        SocialMediaMarketing,
        PayPerClick,
        ContentMarketing,
        GraphicDesigners,
        ChatbotDevelopment,
        MobileAppMarketingAgencies,
        EmailMarketingAgencies,
        SEMAgencies,
        BrandingAgencies,
        PRAgencies,
        DigitalStrategyAgencies,
        VideoProductionAgencies
    }
}
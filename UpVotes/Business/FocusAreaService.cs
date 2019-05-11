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

        public int GetSoftwareCategoryID(string urlCategoryName)
        {
            return GetSoftwareCategoryIDByEnum(urlCategoryName);
        }

        private int GetSoftwareCategoryIDByEnum(string urlCategoryName)
        {
            int softwareCategoryID = 0;
            switch (urlCategoryName.Trim())
            {
                case "mobile-app-softwares":
                    softwareCategoryID = (int)SoftwareCategories.MobileAppSoftwares;
                    break;
                case "seo-softwares":
                    softwareCategoryID = (int)SoftwareCategories.SeoSoftwares;
                    break;
                case "social-media-softwares":
                    softwareCategoryID = (int)SoftwareCategories.SocialMediaSoftwares;
                    break;
                case "content-marketing-softwares":
                    softwareCategoryID = (int)SoftwareCategories.ContentMarketingSoftwares;
                    break;
                case "app-design-softwares":
                    softwareCategoryID = (int)SoftwareCategories.AppDesignSoftwares;
                    break;
                case "ecommerce-softwares":
                    softwareCategoryID = (int)SoftwareCategories.ECommerceSoftwares;
                    break;
                case "marketing-softwares":
                    softwareCategoryID = (int)SoftwareCategories.MarketingSoftwares;
                    break;
                case "cms-softwares":
                    softwareCategoryID = (int)SoftwareCategories.CMSSoftwares;
                    break;
                case "email-marketing-softwares":
                    softwareCategoryID = (int)SoftwareCategories.EmailMarketingSoftwares;
                    break;
                case "game-development-softwares-":
                    softwareCategoryID = (int)SoftwareCategories.GameDevelopmentSoftwares;
                    break;
                case "graphic-design-softwares":
                    softwareCategoryID = (int)SoftwareCategories.GraphicDesignSoftwares;
                    break;
                case "iot-softwares":
                    softwareCategoryID = (int)SoftwareCategories.IotSoftwares;
                    break;
                case "artificial-intelligence-softwares":
                    softwareCategoryID = (int)SoftwareCategories.ArtificialIntelligenceSoftwares;
                    break;
                case "data-visualization-softwares":
                    softwareCategoryID = (int)SoftwareCategories.DataVisualizationSoftwares;
                    break;
                case "inventory-management-softwares":
                    softwareCategoryID = (int)SoftwareCategories.InventoryManagementSoftwares;
                    break;
                case "accounting-softwares":
                    softwareCategoryID = (int)SoftwareCategories.AccountingSoftwares;
                    break;
                case "workflow-management-softwares":
                    softwareCategoryID = (int)SoftwareCategories.WorkflowManagementSoftwares;
                    break;
                case "crm-softwares":
                    softwareCategoryID = (int)SoftwareCategories.CRMSoftwares;
                    break;
                case "business-intelligence-softwares-":
                    softwareCategoryID = (int)SoftwareCategories.BusinessIntelligenceSoftwares;
                    break;
                case "machine-learning-softwares":
                    softwareCategoryID = (int)SoftwareCategories.MachineLearningSoftwares;
                    break;
                case "product-management-softwares":
                    softwareCategoryID = (int)SoftwareCategories.ProductManagementSoftwares;
                    break;
                case "erp-softwares":
                    softwareCategoryID = (int)SoftwareCategories.ERPSoftwares;
                    break;
                case "human-resource-softwares":
                    softwareCategoryID = (int)SoftwareCategories.HumanResourceSoftwares;
                    break;
                default:
                    softwareCategoryID = (int)SoftwareCategories.MobileAppSoftwares;
                    break;
            }

            return softwareCategoryID;
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
    public enum SoftwareCategories
    {
        MobileAppSoftwares=1,
        SeoSoftwares,
        SocialMediaSoftwares,
        ContentMarketingSoftwares,
        AppDesignSoftwares,
        ECommerceSoftwares,
        MarketingSoftwares,
        CMSSoftwares,
        EmailMarketingSoftwares,
        GameDevelopmentSoftwares,
        GraphicDesignSoftwares,
        IotSoftwares,
        ArtificialIntelligenceSoftwares,
        DataVisualizationSoftwares,
        InventoryManagementSoftwares,
        AccountingSoftwares,
        WorkflowManagementSoftwares,
        CRMSoftwares,
        BusinessIntelligenceSoftwares,
        MachineLearningSoftwares,
        ProductManagementSoftwares,
        ERPSoftwares,
        HumanResourceSoftwares
    }
}
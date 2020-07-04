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
                    focusAreaID = (int)FocusAreas.MobileAppDevelopment;
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
                    focusAreaID = (int)FocusAreas.SoftwareDevelopment;
                    break;

                case "web-development-companies":
                    focusAreaID = (int)FocusAreas.WebDevelopment;
                    break;
                case "ui-ux-agencies":
                    focusAreaID = (int)FocusAreas.UXUIDesign;
                    break;
                case "wearable-application-developers":
                    focusAreaID = (int)FocusAreas.WearableAppDevelopment;
                    break;
                case "ecommerce-developers":
                    focusAreaID = (int)FocusAreas.EcommerceDevelopment;
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
                    focusAreaID = (int)FocusAreas.IOTDevelopment;
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
                case "human-resources-companies":
                    focusAreaID = (int)FocusAreas.HumanResources;
                    break;
                case "blockchain-developers":
                    focusAreaID = (int)FocusAreas.BlockchainDevelopers;
                    break;
                case "software-testing-companies":
                    focusAreaID = (int)FocusAreas.ApplicationTesting;
                    break;
                case "logo-design-agencies":
                    focusAreaID = (int)FocusAreas.LogoDesignAgencies;
                    break;
                case "print-design-agencies":
                    focusAreaID = (int)FocusAreas.PrintDesignAgencies;
                    break;
                case "product-design-agencies":
                    focusAreaID = (int)FocusAreas.ProductDesignAgencies;
                    break;
                case "packaging-design-agencies":
                    focusAreaID = (int)FocusAreas.PackagingDesignAgencies;
                    break;
                case "affiliate-marketing-agencies":
                    focusAreaID = (int)FocusAreas.AffiliateMarketingAgencies;
                    break;
                case "artificial-intelligence-companies":
                    focusAreaID = (int)FocusAreas.ArtificialIntelligence;
                    break;
                case "cloud-consulting-companies":
                    focusAreaID = (int)FocusAreas.CloudConsultants;
                    break;
                default:
                    focusAreaID = (int)FocusAreas.MobileAppDevelopment;
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
                case "game-development-softwares":
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
                case "business-intelligence-softwares":
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
        WebDesign = 2,
        SoftwareDevelopment = 3,
        IOTDevelopment = 4,
        WebDevelopment = 5,
        //CustomSoftwareDevelopemt = 6,
        SearchEngineOptimization = 7,
        UXUIDesign = 8,
        MobileAppDevelopment = 9,
        EcommerceDevelopment = 10,
        WearableAppDevelopment = 11,
        ARVRApplicationDevelopment = 12,
        SocialMediaMarketing = 13,
        PayPerClick = 14,
        ContentMarketing = 15,
        GraphicDesigners = 16,
        ChatbotDevelopment = 17,
        MobileAppMarketingAgencies = 18,
        EmailMarketingAgencies = 19,
        SEMAgencies = 20,
        BrandingAgencies = 21,
        PRAgencies = 22,
        DigitalStrategyAgencies = 23,
        VideoProductionAgencies = 24,
        HumanResources=27,
        BlockchainDevelopers = 28,
        ApplicationTesting = 29,
        LogoDesignAgencies = 30,
        PrintDesignAgencies = 31,
        ProductDesignAgencies = 32,
        PackagingDesignAgencies = 33,
        AffiliateMarketingAgencies = 34,
        ArtificialIntelligence = 35,
        CloudConsultants = 36
    }
    public enum SoftwareCategories
    {
        MobileAppSoftwares=1,
        SeoSoftwares = 2,
        SocialMediaSoftwares = 3,
        ContentMarketingSoftwares = 4,
        AppDesignSoftwares = 5,
        ECommerceSoftwares = 6,
        MarketingSoftwares = 7,
        CMSSoftwares = 8,
        EmailMarketingSoftwares = 9,
        GameDevelopmentSoftwares = 10,
        GraphicDesignSoftwares = 11,
        IotSoftwares = 12,
        ArtificialIntelligenceSoftwares = 13,
        DataVisualizationSoftwares = 14,
        InventoryManagementSoftwares = 15,
        AccountingSoftwares = 16,
        WorkflowManagementSoftwares = 17,
        CRMSoftwares = 18,
        BusinessIntelligenceSoftwares = 19,
        MachineLearningSoftwares = 20,
        ProductManagementSoftwares = 21,
        ERPSoftwares = 22,
        HumanResourceSoftwares = 23
    }
}
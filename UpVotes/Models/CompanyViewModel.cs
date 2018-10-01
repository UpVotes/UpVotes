using System.Collections.Generic;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Models
{
    public class CompanyViewModel
    {
        public string CompanyFocusData { get; set; }

        public List<CompanyEntity> CompanyList { get; set; }

        public List<FocusAreaEntity> FocusAreaList { get; set; }

        public List<CompanyFocusEntity> PrimaryCompanyFocus { get; set; }
        public List<CompanyFocusEntity> IndustialCompanyFocus { get; set; }
        public List<CompanyFocusEntity> CompanyClientFocus { get; set; }
        public List<string> SubfocusNames { get; set; }
        public List<CompanyFocusEntity> CompanySubFocus { get; set; }
        public int AverageUserRating { get; set; }

        public int TotalNoOfUsers { get; set; }        

        public string WebBaseURL { get; set; }

        public string CategoryHeadLine { get; set; }

        public string Title { get; set; }

        public string MetaTag { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }

        public int PageIndex { get; set; }

        public int TotalNoOfCompanies { get; set; }
        public string SubFocusArea { get; set; }
        public string Location { get; set; }
        public List<ClaimInfoDetail> ClaimList { get; set; }
    }
    public class CategoryMetaTagsDetails
    {
        public int CategoryBasedMetaTagsID { get; set; }
        public string FocusAreaName { get; set; }
        public string SubFocusAreaName { get; set; }
        public string Title { get; set; }
        public string TwitterTitle { get; set; }
        public string Descriptions { get; set; }
    }
}
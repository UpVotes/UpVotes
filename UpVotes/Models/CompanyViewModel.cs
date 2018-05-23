using System.Collections.Generic;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Models
{
    public class CompanyViewModel
    {
        public string CompanyFocusData { get; set; }

        public List<CompanyEntity> CompanyList { get; set; }
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
    }
}
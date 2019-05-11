using System;
using System.Collections.Generic;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Models
{
    public class CompanySoftwareReviewViewModel
    {
        public int CompanyReviewID { get; set; }
        public int SoftwareReviewID { get; set; }

        public int CompanyID { get; set; }
        public int SoftwareID { get; set; }

        public string CompanyName { get; set; }
        public string SoftwareName { get; set; }

        public int FocusAreaID { get; set; }
        public int ServiceCategoryID { get; set; }

        public string ProjectName { get; set; }

        public string FeedBack { get; set; }

        public int Rating { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string ReviewerCompanyName { get; set; }

        public string Designation { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime ReviewDate { get; set; }        
    }
    public class CompanySoftwareUserReviews
    {
        public List<CompanyReviewsEntity> ReviewsList { get; set; }
        public List<string> CompanyNamesList { get; set; }
        public List<SoftwareReviewsEntity> SoftwareReviewsList { get; set; }
        public List<string> SoftwareNamesList { get; set; }

    }
}
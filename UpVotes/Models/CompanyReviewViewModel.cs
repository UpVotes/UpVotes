using System;
using System.Collections.Generic;

namespace UpVotes.Models
{
    public class CompanyReviewViewModel
    {
        public int CompanyReviewID { get; set; }

        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public int FocusAreaID { get; set; }

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
}
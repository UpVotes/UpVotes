using System.Collections.Generic;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Models
{
    public class CompanyViewModel
    {
        public string CompanyFocusData { get; set; }

        public List<CompanyEntity> CompanyList { get; set; }

        public int AverageUserRating { get; set; }

        public int TotalNoOfUsers { get; set; }

        public CompanyReviewsEntity CompanyReview { get; set; }

        public string WebBaseURL { get; set; }

        public string CategoryHeadLine { get; set; }
    }
}
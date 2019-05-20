using System.Collections.Generic;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Models
{
    public class SoftwareViewModel
    {
        public List<SoftwareEntity> SoftwareList { get; set; }
       
        public int AverageUserRating { get; set; }

        public int TotalNoOfUsers { get; set; }        

        public string WebBaseUrl { get; set; }

        public string CategoryHeadLine { get; set; }

        public string Title { get; set; }

        public string MetaTag { get; set; }

        public int PageCount { get; set; }

        public int PageNumber { get; set; }

        public int PageIndex { get; set; }

        public int TotalNoOfSoftwares { get; set; }

        public string SoftwareCategoryId { get; set; }

        public int LoggedInUser { get; set; }

        public bool IsAdmin { get; set; }

        public List<OverviewNewsResponseEntity> OverviewNewsData { get; set; }
    }    
}
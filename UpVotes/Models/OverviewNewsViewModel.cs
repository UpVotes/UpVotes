using System.Collections.Generic;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Models
{
    public class OverviewNewsViewModel
    {      
        public List<OverviewNewsResponseEntity> OverviewNewsData { get; set; }  
        public string Title { get; set; }      
    }
}
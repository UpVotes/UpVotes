using System.Collections.Generic;
using UpVotes.BusinessEntities.Entities;

namespace UpVotes.Models
{
    public class SubmitReviewViewModel
    {
        public int TabIndex { get; set; }
        public string SoftwareOrCompanyName { get; set; }
        public int SoftwareOrCompanyId { get; set; }
    }
    
}
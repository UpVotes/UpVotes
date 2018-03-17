using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UpVotes.Models
{
    public class TwitterLinkedInLoginModel
    {
        public string firstName { get; set; }

        public string id { get; set; }

        public string lastName { get; set; }

        public string pictureUrl { get; set; }

        public string publicProfileUrl { get; set; }

        public int userType { get; set; }
    }
}
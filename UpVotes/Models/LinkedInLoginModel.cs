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

    public class LinkedInInfo
    {
        public string localizedLastName { get; set; }
        public string localizedFirstName { get; set; }
        public string id { get; set; }
    }

    public class LinkedInImageInfo
    {
        public profilePictures profilePicture { get; set; }
       //public linkedimageElements profilePicture { get;set; }
    }

    public class profilePictures
    {
     public string displayImage { get; set; }
     public DisplayImage displayImagetilde { get; set; }
    }

    public class DisplayImage
    {
        public List<Element> elements { get; set; }
    }
    public class Element
    {
        public string artifact { get; set; }
        public List<Identifier> identifiers { get; set; }
    }
    public class Identifier
    {
        public string identifier { get; set; }
    }

    public class RegisteredUser
    {
        public string WorkEmailID { get; set; }
        public string Password { get; set; }        
    }
    
}
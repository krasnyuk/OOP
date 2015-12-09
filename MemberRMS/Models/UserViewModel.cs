using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberRMS.Models
{
    public class UserViewModel : Controller
    {

        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Telephone { get; set; }
        public string Mail { get; set; }
        public string Gender { get; set; }
    }
}
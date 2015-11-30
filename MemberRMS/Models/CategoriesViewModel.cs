using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberRMS.Models
{
    public class CategoriesViewModel
    {
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public int? ParentCategoryID { get; set; }
    }
}
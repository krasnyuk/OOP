using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MemberRMS.Models
{
    public class Login
    {
        //создание поляе в модели, для использования в контроллере
        public string Username { get; set; }
        public string Psssword { get; set; }
        public bool RememberMe { get; set; }

    }
}
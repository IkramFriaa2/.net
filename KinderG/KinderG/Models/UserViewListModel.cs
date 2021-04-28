using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KinderGPI.Models
{
    public class UserViewModel
    {
        public User usr { get; set; }
        public IList<User> users;
    }
}
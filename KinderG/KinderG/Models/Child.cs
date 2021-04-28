using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KinderGPI.Models
{
    public class Child 
    {
        public long id { get; set; }
       
        [Display(Name = "Firstname ")]
        public string firstname { get; set; }
        [Display(Name = "Lastname ")]
        public string lastname { get; set; }
        [Display(Name = "Age ")]
        public int age { get; set; }
        [Display(Name = "Sex ")]
        public string sex { get; set; }
    
    }
}
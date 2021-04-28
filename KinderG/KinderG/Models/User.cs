using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KinderGPI.Models
{
    public class User
    {

        public long id { get; set; }
        [Required]
        [Display(Name = "How much is this ? ")]
        public string Captcha { get; set; }
        [Display(Name = "Username ")]
        public string username { get; set; }
        [Display(Name = "Email ")]
        public string email { get; set; }
        [Display(Name = "Password ")]
        public string password { get; set; }
        [Display(Name = "Who are you ? ")]
        public List<String> roles { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public Boolean isAuthorized { get; set; }
      
        public User()
        {
            isAuthorized = false;
        }
    }
    public enum Role
    {
        ROLE_ADMIN, ROLE_PARENT, ROLE_DOCTOR, ROLE_DIRECTOR
    }
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class JWT
    {
        public string Token { get; set; }
    }
    /*  public class Parent : User
      {
          public int idP { get; set; }
          [DataType(DataType.Date)]
          [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
          [Display(Name = " Date Register")]
          public DateTime dateReg { get; set; }

          public String adresse { get; set; }
          private String etatCivil { get; set; }
      }*/
}


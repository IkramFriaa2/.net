
using KinderGPI.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KinderGPI.Controllers
{
    public class ChildController : Controller
    {
        // GET: Child
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Profile(Child usr)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8086");
                var postJob = client.PostAsJsonAsync<Child>("users/addChild", usr);
                postJob.Wait();
                var postResult = postJob.Result;
             /*   if (postResult.IsSuccessStatusCode)
                    return View("Profile", usr);*/
            }
           
            return View(usr);
        }
    }
}
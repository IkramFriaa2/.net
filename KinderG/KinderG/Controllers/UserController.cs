
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

public class UserController : Controller
{

    public async Task<ActionResult> Index()
    {
        return View();
    }

    // HTTP GET VERSION
    public async Task<System.Web.Mvc.ActionResult> Create()
    {
        return View();
    }
    public async Task<System.Web.Mvc.ActionResult> Signin()
    {
        return View();
    }


    [HttpPost]
    public ActionResult Signin(User usr)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:8086");
            var postJob = client.PostAsJsonAsync<User>("users/signin", usr);
            postJob.Wait();
            var postResult = postJob.Result;
            if (postResult.IsSuccessStatusCode)
            {
                usr.isAuthorized = true;
                var getJob = client.GetAsync("users/find/" + usr.username);
                User resUser = getJob.Result.Content.ReadAsAsync<User>().Result;
                Session["User"] = resUser;
               
  
                return View("Thanks", resUser);
            }
             ModelState.AddModelError(string.Empty, "");
        }
       
        return View(usr);
    }

public async Task<System.Web.Mvc.ActionResult> Search()
{
    return View();
}




// HTTP POST VERSION  
[HttpPost]
    public ActionResult Create(User usr)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:8086");
            var postJob = client.PostAsJsonAsync<User>("users/signup", usr);
            postJob.Wait();
            var postResult = postJob.Result;
            if (postResult.IsSuccessStatusCode)
                return View("Ok", usr);
        }
        ModelState.AddModelError(string.Empty, "Server occured ");
        return View(usr);
    }
    public ActionResult Details(int id)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:8086");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = client.GetAsync("/users/" + id).Result;

        User Users = response.Content.ReadAsAsync<User>().Result;

        return View(Users);
    }
   
  
  
    public ActionResult ListUsers()
    {

        IList<User> usr = null;
        UserViewModel userView = new UserViewModel() ;


        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:8086");
            var responseTask = client.GetAsync("users/allusers");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                User usrSession = Session["User"] as User;
                userView.usr = usrSession;

                var readJob = result.Content.ReadAsAsync<IList<User>>();
                //readJob.Wait();
                usr = readJob.Result;
                userView.users = usr;
                // return View("Ok",usrSession);
                return View(userView);
            }
        }
         return View("Ok");
        
    }

    /*
    [Authorize]
    public ActionResult ChangePassword()
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    public ActionResult ChangePassword(ChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {

            // ChangePassword will throw an exception rather
            // than return false in certain failure scenarios.
            bool changePasswordSucceeded;
            try
            {
                MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline );
            /*    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
            }
            catch (Exception)
            {
                changePasswordSucceeded = false;
            }

            if (changePasswordSucceeded)
            {
                return RedirectToAction("ChangePasswordSuccess");
            }
            else
            {
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    public ActionResult ChangePasswordSuccess()
    {
        return View();
    }
    */
    public ViewResult GetReservation() => View();


    [HttpPost]
    public ActionResult LogOn(User usr)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:8086");
            var postJob = client.PostAsJsonAsync<User>("users/signin/", usr);
            postJob.Wait();
            var postResult = postJob.Result;
            if (postResult.IsSuccessStatusCode)
                return View("Thanks", usr);
        }
        ModelState.AddModelError(string.Empty, "Server occured ");
        return View(usr);
    }
    /*
    public ActionResult Login()
    {
        string baseUrl = "http://localhost:8086";
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(baseUrl);
        var contentType = new MediaTypeWithQualityHeaderValue
    ("application/json");
        client.DefaultRequestHeaders.Accept.Add(contentType);

        User userModel = new User();
        userModel.username = "ikramfriaa12";
        userModel.password = "karrouma";

        string stringData = JsonConvert.SerializeObject(userModel);
        var contentData = new StringContent(stringData,
    System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = client.PostAsync
    ("/users/signin", contentData).Result;
        string stringJWT = response.Content.
    ReadAsStringAsync().Result;
        JWT jwt = JsonConvert.DeserializeObject
    <JWT>(stringJWT);

      //  HttpContext.Session.SetString ("token", jwt.Token);


        ViewBag.Message = "User logged in successfully!";

        return View("Index");
    }
    public ActionResult Logout()
    {
        HttpContext.Session.Remove("token");
        ViewBag.Message = "User logged out successfully!";
        return View("Index");


    }*/
    public ActionResult Delete(int id)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:8086/");

            //HTTP DELETE
            var deleteTask = client.DeleteAsync("users/delete/" + id.ToString());
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {

                TempData["msg"] = "<script>alert('Change succesfully');</script>";
            }
        }

        return RedirectToAction("ListUsers");
    }
    public ActionResult Index(string searchString)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:8086/users/");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response;
        if (!String.IsNullOrEmpty(searchString))
        {
            System.Diagnostics.Debug.Write("searchsearch");
            response = client.GetAsync("find/" + searchString).Result;
        }
        else
        {
            System.Diagnostics.Debug.Write("simplesimple");
            response = client.GetAsync("stock").Result;
        }

        IEnumerable<User> usr = response.Content.ReadAsAsync<IEnumerable<User>>().Result;
        ViewBag.stocks = usr;
        return View();
    }

    public ActionResult Edit(User usr)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:8086/users/");

            var responseTask = client.PutAsJsonAsync(string.Format("update/{0}", usr.id), usr);
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = string.Format("User: {0} is successfully edited", usr.username);
                TempData["SuccessMessage"] = string.Format("User: {0} is successfully edited", usr.email);
            }
            else
            {

            }
            return View(usr);
        }
    }
    /*
    public ActionResult Details(int id)
    {
        IList<User> usr = null;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:8086/users");

            var responseTask = client.GetAsync(string.Format("/{0}", id));
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readJob = result.Content.ReadAsAsync<IList<User>>();
                //readJob.Wait();
                usr = readJob.Result;
            }
            else
            {
                // usr = new User();
                //TempData["ErrorMessage"] = string.Format(Resources.ERR_SERVER_ERROR, "viewing");
            }
        }

        return View(usr);
    }
    */
    public ActionResult CaptchaImage(string prefix, bool noisy = true)
    {
        var rand = new Random((int)DateTime.Now.Ticks);
        //generate new question 
        int a = rand.Next(10, 99);
        int b = rand.Next(0, 9);
        var captcha = string.Format("{0} + {1} = ?", a, b);

        //store answer 
        Session["Captcha" + prefix] = a + b;

        //image stream 
        FileContentResult img = null;

        using (var mem = new System.IO.MemoryStream())
        using (var bmp = new System.Drawing.Bitmap(130, 30))
        using (var gfx = System.Drawing.Graphics.FromImage((Image)bmp))
        {
            gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            gfx.SmoothingMode = SmoothingMode.AntiAlias;
            gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

            //add noise 
            if (noisy)
            {
                int i, r, x, y;
                var pen = new Pen(Color.Yellow);
                for (i = 1; i < 10; i++)
                {
                    pen.Color = Color.FromArgb(
                    (rand.Next(0, 255)),
                    (rand.Next(0, 255)),
                    (rand.Next(0, 255)));

                    r = rand.Next(0, (130 / 3));
                    x = rand.Next(0, 130);
                    y = rand.Next(0, 30);

                    gfx.DrawEllipse(pen, x - r, y - r, r, r);
                }
            }

            //add question 
            gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

            //render as Jpeg 
            bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
            img = this.File(mem.GetBuffer(), "image/Jpeg");
        }

        return img;
    }
    [HttpPost]
    public ActionResult GetReservation(String username)
    {
        User usr = new User();
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://localhost:8086/users/");
            var responseTask = client.GetAsync("search/{username}");
            //using (var response = await httpClient.GetAsync("http://localhost:8086/users/search/" + username))
            {
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<IList<User>>();
                    //usr = JsonConvert.DeserializeObject<User>(apiResponse);
                }
                //  else
                //    ViewBag.StatusCode = response.StatusCode;
            }
        }
        return View(usr);
    }

    public ViewResult AddFile() => View();

    [HttpPost]
    public async Task<ActionResult> AddFile(IFormFile file)
    {
        string apiResponse = "";
        using (var httpClient = new HttpClient())
        {
            var form = new MultipartFormDataContent();
            using (var fileStream = file.OpenReadStream())
            {
                form.Add(new StreamContent(fileStream), "file", file.FileName);
                using (var response = await httpClient.PostAsync("https://localhost:8086/uploadFile", form))
                {
                    response.EnsureSuccessStatusCode();
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
        }
        return View((object)apiResponse);
    }
}

/* public ActionResult LogOff()
 {
     FormsAuthentication.SignOut();

     return RedirectToAction("Index", "Home");
 }*/

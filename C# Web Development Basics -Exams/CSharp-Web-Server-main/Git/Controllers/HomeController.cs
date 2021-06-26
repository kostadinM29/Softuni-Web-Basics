using System;
using System.Collections.Generic;
using System.Text;
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace Git.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponse Index()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect("/Repositories/All");
            }

            return View();
        }
    }
}

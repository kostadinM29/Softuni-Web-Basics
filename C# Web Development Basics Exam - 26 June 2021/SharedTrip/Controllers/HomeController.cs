namespace SharedTrip.Controllers
{
    using MyWebServer.Controllers;
    using MyWebServer.Http;

    public class HomeController : Controller
    {
        public HttpResponse Index()
        {
            return this.User.IsAuthenticated ? this.Redirect("/Trips/All") : View(); // Not sure if i should throw 401 or just redirect.
        }
    }
}
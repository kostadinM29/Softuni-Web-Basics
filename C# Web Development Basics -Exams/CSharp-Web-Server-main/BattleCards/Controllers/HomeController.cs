using MyWebServer.Controllers;
using MyWebServer.Http;

namespace BattleCards.Controllers
{
    

    public class HomeController : Controller
    {
        public HttpResponse Index()
        {
            return this.User.IsAuthenticated ? this.Redirect("/Cards/All") : View();
        }
    }
}
namespace SharedTrip.Controllers
{
    using Data;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using Services;
    using SharedTrip.Data.Models;
    using System.Linq;
    using ViewModels.UserModels;
    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly IPasswordHasher passwordHasher;
        private readonly SharedTripDbContext data;

        public UsersController(
            IValidator validator,
            IPasswordHasher passwordHasher,
            SharedTripDbContext data)
        {
            this.validator = validator;
            this.passwordHasher = passwordHasher;
            this.data = data;
        }

        public HttpResponse Register() => User.IsAuthenticated ? Redirect("/Trips/All") : View(); // Not sure if i should throw 401 or just redirect.


        [HttpPost]
        public HttpResponse Register(UserRegisterModel model)
        {
            if (User.IsAuthenticated)
            {
                return Redirect("/Trips/All");
            }
            var modelErrors = this.validator.ValidateUser(model);

            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                modelErrors.Add($"User with '{model.Username}' username already exists.");
            }

            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                modelErrors.Add($"User with '{model.Email}' e-mail already exists.");
            }

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var user = new User
            {
                Username = model.Username,
                Password = this.passwordHasher.HashPassword(model.Password),
                Email = model.Email,
            };

            data.Users.Add(user);

            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        public HttpResponse Login() => User.IsAuthenticated ? Redirect("/Trips/All") : View(); // Not sure if i should throw 401 or just redirect.

        [HttpPost]
        public HttpResponse Login(UserLoginModel model)
        {
            if (User.IsAuthenticated)
            {
                return Redirect("/Trips/All");
            }

            var hashedPassword = this.passwordHasher.HashPassword(model.Password);

            var userId = this.data
                .Users
                .Where(u => u.Username == model.Username && u.Password == hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId == null)
            {
                return Error("Username and password combination is not valid.");
            }

            this.SignIn(userId);

            return Redirect("/Trips/All");
        }
        [Authorize]
        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}

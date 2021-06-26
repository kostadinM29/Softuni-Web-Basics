using CarShop.Data.Models;

namespace CarShop.Controllers
{
    using CarShop.Data;
    using CarShop.Models.Issues;
    using CarShop.Services;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using System.Linq;

    public class IssuesController : Controller
    {
        private readonly IUserService userService;
        private readonly CarShopDbContext data;

        public IssuesController(IUserService userService, CarShopDbContext data)
        {
            this.userService = userService;
            this.data = data;
        }

        [Authorize]
        public HttpResponse Delete(string issueId, string carId)
        {
            var issue = data.Issues.FirstOrDefault(i => i.Id == issueId && i.CarId == carId);

            if (issue == null)
            {
                return Error($"Issue or car not found");
            }

            this.data.Issues.Remove(issue);

            this.data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={issue.CarId}");
        }

        [Authorize]
        public HttpResponse Fix(string issueId, string carId)
        {
            var user = data.Users.First(u => u.Id == User.Id);

            if (!user.IsMechanic)
            {
                return Unauthorized();
            }

            var issue = data.Issues.FirstOrDefault(i => i.Id == issueId && i.CarId == carId);

            if (issue == null)
            {
                return Error($"Issue or car not found");
            }

            issue.IsFixed = true;

            data.Issues.Update(issue);

            data.SaveChanges();

            return Redirect($"/Issues/CarIssues?CarId={issue.CarId}");
        }

        [Authorize]
        public HttpResponse Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddIssueFormModel model, string carId)
        {
            var issueToAdd = new Issue()
            {
                Description = model.Description,
                CarId = carId
            };

            this.data.Issues.Add(issueToAdd);

            this.data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={issueToAdd.CarId}");
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                var userOwnsCar = this.data.Cars
                    .Any(c => c.Id == carId && c.OwnerId == this.User.Id);

                if (!userOwnsCar)
                {
                    return Error("You do not have access to this car.");
                }
            }

            var carWithIssues = this.data
                .Cars
                .Where(c => c.Id == carId)
                .Select(c => new CarIssuesViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    Issues = c.Issues.Select(i => new IssueListingViewModel
                    {
                        Id = i.Id,
                        IsMechanic = userService.IsMechanic(User.Id),
                        Description = i.Description,
                        IsFixed = i.IsFixed
                    })
                })
                .FirstOrDefault();

            if (carWithIssues == null)
            {
                return Error($"Car with ID '{carId}' does not exist.");
            }

            return View(carWithIssues);
        }
    }
}

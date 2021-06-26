using System;
using System.Globalization;
using System.Linq;
using Git.Data;
using Git.Data.Models;
using Git.Services;
using Git.ViewModels.RepositoriesModels;
using Microsoft.EntityFrameworkCore;
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext data;

        public RepositoriesController(
            IValidator validator,
            GitDbContext data)
        {
            this.validator = validator;
            this.data = data;
        }

        [Authorize]
        public HttpResponse Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(RepositoriesCreateModel model)
        {
            var modelErrors = this.validator.ValidateRepository(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var repo = new Repository()
            {
                Name = model.Name,
                CreatedOn = DateTime.UtcNow,
                IsPublic = model.RepositoryType == "Public", // potential error
                OwnerId = User.Id
            };

            data.Repositories.Add(repo);

            data.SaveChanges();

            return Redirect("/Repositories/All");
        }


        public HttpResponse All()
        {
            var repos = data.Repositories.Where(r => r.IsPublic || User.Id == r.OwnerId).AsQueryable()
                .Select(r => new RepositoriesAllModel
                {
                    Name = r.Name,
                    Owner = r.Owner.Username,
                    CreatedOn = r.CreatedOn.AddHours(2).ToString("g"),// can make it show bg time but meh
                    CommitCount = !User.IsAuthenticated ? r.Commits.Count : r.Commits.Count(c => c.CreatorId == User.Id),
                    Id = r.Id
                })
                .ToList();

            return View(repos);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git.Data;
using Git.Data.Models;
using Git.Services;
using Git.ViewModels.CommitsModels;
using Git.ViewModels.RepositoriesModels;
using Microsoft.EntityFrameworkCore;
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext data;

        public CommitsController(
            IValidator validator,
            GitDbContext data)
        {
            this.validator = validator;
            this.data = data;
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var repo = data.Repositories.First(r => r.Id == id);
            var repoView = new CommitCreateRepositoryModel()
            {
                Name = repo.Name,
                Id = repo.Id
            };
            return View(repoView);
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CommitsCreateModel model, string id)
        {
            var modelErrors = validator.ValidateCommit(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var commit = new Commit()
            {
                Description = model.Description,
                CreatedOn = DateTime.UtcNow,
                RepositoryId = id,
                CreatorId = User.Id,
            };

            this.data.Commits.Add(commit);

            this.data.SaveChanges();

            return Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = data.Commits.Where(c => c.CreatorId == User.Id).AsQueryable()
                .Select(c => new CommitsAllModel()
                {
                    RepositoryName = c.Repository.Name,
                    Description = c.Description,
                    CreatedOn = c.CreatedOn.ToString("g"),
                    Id = c.Id
                })
                .ToList();

            return View(commits);
        }
        [Authorize]
        public HttpResponse Delete(string id)
        {
            var repositoryOwnerId = data.Commits.Where(x => x.Id == id).Select(x => x.Repository.OwnerId)
                .FirstOrDefault();
            if (repositoryOwnerId != User.Id)
            {
                return Redirect("/Commits/All");
            }
            var commit = data.Commits.Find(id);
            data.Commits.Remove(commit);
            data.SaveChanges();
            return Redirect("/Repositories/All");
        }
    }
}

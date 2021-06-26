using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git.ViewModels.CommitsModels;
using Git.ViewModels.RepositoriesModels;
using Git.ViewModels.UserModels;

namespace Git.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(UserRegisterModel model);

        ICollection<string> ValidateRepository(RepositoriesCreateModel model);

        ICollection<string> ValidateCommit(CommitsCreateModel model);
    }
}

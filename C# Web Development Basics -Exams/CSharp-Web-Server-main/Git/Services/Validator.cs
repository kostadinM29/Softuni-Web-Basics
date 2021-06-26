using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Git.ViewModels.CommitsModels;
using Git.ViewModels.RepositoriesModels;
using Git.ViewModels.UserModels;

namespace Git.Services
{
    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(UserRegisterModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < 5 || model.Username.Length > 20)
            {
                errors.Add($"Username '{model.Username}' is not valid. It must be between 5 and 20 characters long.");
            }

            if (!Regex.IsMatch(model.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                errors.Add($"Email {model.Email} is not a valid e-mail address.");
            }

            if (model.Password.Length < 6 || model.Password.Length > 20)
            {
                errors.Add($"The provided password is not valid. It must be between 6 and 20 characters long.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Password and its confirmation are different.");
            }

            return errors;
        }
        public ICollection<string> ValidateRepository(RepositoriesCreateModel model)
        {
            var errors = new List<string>();

            if (model.Name.Length < 3 || model.Name.Length > 10)
            {
                errors.Add($"Repository '{model.Name}' is not valid. It must be between 3 and 10 characters long.");
            }

            return errors;
        }

        public ICollection<string> ValidateCommit(CommitsCreateModel model)
        {
            var errors = new List<string>();

            if (model.Description.Length < 5 )
            {
                errors.Add($"Description '{model.Description}' is not valid. It must be more than 5 characters long.");
            }

            return errors;
        }
    }
    
}

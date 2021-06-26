using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BattleCards.ViewModels.CardModels;
using BattleCards.ViewModels.UserModels;

namespace BattleCards.Services
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

        public ICollection<string> ValidateCard(CardAddFormModel model)
        {
            var errors = new List<string>();

            if (model.Name.Length < 5 || model.Name.Length > 15)
            {
                errors.Add($"Username '{model.Name}' is not valid. It must be between 5 and 15 characters long.");
            }

            //if (!Uri.IsWellFormedUriString(model.ImageUrl, UriKind.Absolute))
            //{
            //    errors.Add($"Image {model.ImageUrl} is not a valid URL.");
            //}

            if (model.Attack < 0)
            {
                errors.Add("Attack cannot be negative.");
            }

            if (model.Health < 0)
            {
                errors.Add("Health cannot be negative.");
            }

            if (model.Description.Length > 200 || model.Description == null) // not sure if it can be null
            {
                errors.Add("Description is not valid. It must be between 1 and 200 characters long");
            }

            return errors;
        }
    }
}

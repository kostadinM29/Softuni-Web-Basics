namespace SharedTrip.Services
{
    using ViewModels.TripModels;
    using ViewModels.UserModels;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
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

        public ICollection<string> ValidateTrip(TripsAddFormModel model)
        {
            var errors = new List<string>();
            if (model.Seats <2 || model.Seats > 6)
            {
                errors.Add($"Seat count not valid. It must be between 2 and 6.");
            }

            if (!Regex.IsMatch(model.DepartureTime, @"([0-9]{2})\.([0-9]{2})\.([0-9]{4})\ ([0-9]{2}):([0-9]{2})"))
            {
                errors.Add($"Email {model.DepartureTime} is not a valid date format.");
            }

            if (model.Description == null)
            {
                errors.Add("Description cannot be null.");
            }

            if (model.Description == null)
            {
                errors.Add("Description cannot be null.");
            }

            if (model.Description.Length > 80)
            {
                errors.Add("Description character limit reached. It cannot be above 80 characters long.");
            }


            // potential errors :wrong image url- but i won't implement it because from previous exams it has problems with images not ending in jpeg/jpg/png

            return errors;
        }
    }
}

namespace SharedTrip.Services
{
    using System.Collections.Generic;
    using ViewModels.TripModels;
    using ViewModels.UserModels;

    public interface IValidator
    {
        ICollection<string> ValidateUser(UserRegisterModel model);

        ICollection<string> ValidateTrip(TripsAddFormModel model);

    }
}

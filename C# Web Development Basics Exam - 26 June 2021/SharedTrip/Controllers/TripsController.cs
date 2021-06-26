using System;
using System.Globalization;
using SharedTrip.Data.Models;

namespace SharedTrip.Controllers
{
    using System.Linq;
    using MyWebServer.Http;
    using Data;
    using Services;
    using ViewModels.TripModels;

    using MyWebServer.Controllers;
    public class TripsController : Controller
    {
        private readonly IValidator validator;
        private readonly SharedTripDbContext data;

        public TripsController(
            IValidator validator,
           SharedTripDbContext data)
        {
            this.validator = validator;
            this.data = data;
        }

        [Authorize]
        public HttpResponse All() //But how does a user see the trips HE already is in ?? 
        {
            var tripsQuery = data.Trips.AsQueryable();

            var tripsList = tripsQuery // its not mentioned if i should show full trips
                .Select(t => new TripsAllViewModel()
                {
                    TripId = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime =
                        t.DepartureTime.ToString("dd.MM.yyyy HH:mm",
                            CultureInfo.InvariantCulture), // not sure if format has a short
                    Seats = t.Seats,
                })
                .ToList();

            return View(tripsList);
        }

        [Authorize]
        public HttpResponse Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(TripsAddFormModel model)
        {
            var modelErrors = validator.ValidateTrip(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var trip = new Trip()
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.ParseExact(model.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Seats = model.Seats,
                Description = model.Description,
                ImagePath = model.ImagePath,
            };

            data.Trips.Add(trip);
            data.SaveChanges();

            return Redirect("/Trips/All");
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            var tripsQuery = data.Trips.FirstOrDefault(t => t.Id == tripId);

            if (tripsQuery == null)
            {
                return Error("Selected trip doesn't exist.");
            }
            
            var trip = new TripsDetailsViewModel
            {
                ImagePath = tripsQuery.ImagePath,
                TripId = tripsQuery.Id,
                StartPoint = tripsQuery.StartPoint,
                EndPoint = tripsQuery.EndPoint,
                DepartureTime =
                        tripsQuery.DepartureTime.ToString("dd.MM.yyyy HH:mm",
                            CultureInfo.InvariantCulture), // not sure if that format has a short symbol
                Seats = tripsQuery.Seats,
                Description = tripsQuery.Description
            };

            return View(trip);
        }
        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var tripQuery = data.Trips.FirstOrDefault(t => t.Id == tripId);

            if (tripQuery == null)
            {
                return Error("Selected trip doesn't exist."); 
            }

            var userTripQuery = data.UserTrips.FirstOrDefault(ut => ut.TripId == tripId && ut.UserId == User.Id);

            if (userTripQuery != null)
            {
                return Redirect($"/Trips/Details?tripId={tripId}");
            }

            if (tripQuery.Seats == 0)
            {
                return Error("Not enough seats on the trip!"); // Not sure if i should redirect to details or throw error.
            }

            var userTrips = new UserTrip()
            {
                TripId = tripId,
                UserId = User.Id
            };
            tripQuery.Seats--;

            data.UserTrips.Add(userTrips);
            data.SaveChanges();

            return Redirect("/");
        }
    }
}

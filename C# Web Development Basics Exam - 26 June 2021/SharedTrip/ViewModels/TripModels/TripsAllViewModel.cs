namespace SharedTrip.ViewModels.TripModels
{
    using System;
    public class TripsAllViewModel
    {
        public string TripId { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string DepartureTime { get; set; }

        public int Seats { get; set; }
    }
}

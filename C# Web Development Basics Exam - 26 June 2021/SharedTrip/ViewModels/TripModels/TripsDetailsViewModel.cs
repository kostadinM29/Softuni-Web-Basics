namespace SharedTrip.ViewModels.TripModels
{
    public class TripsDetailsViewModel
    {
        public string TripId { get; set; }

        public string ImagePath { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string DepartureTime { get; set; }

        public int Seats { get; set; }

        public string  Description { get; set; }
    }
}

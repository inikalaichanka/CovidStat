using System;

namespace CovidStat.Services.ArrivalsDataProducer.Worker.Models
{
    public class ArrivalViewModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public bool IsVaccinated { get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime? DepartureDate { get; set; }
    }
}

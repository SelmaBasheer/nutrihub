using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.ValueObjects
{
    public class Address
    {
        public string Street { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;

        private Address() { }  // EF Core needs this

        //static factory method
        public static Address Create(
            string street,
            string city,
            string state,
            string postalCode,
            string country)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty");
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty");
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country cannot be empty");

            return new Address
            {
                Street = street,
                City = city,
                State = state,
                PostalCode = postalCode,
                Country = country
            };
        }

        //Useful for logging and displaying address as one clean string
        public override string ToString()
        => $"{Street}, {City}, {State} {PostalCode}, {Country}";
    }
}

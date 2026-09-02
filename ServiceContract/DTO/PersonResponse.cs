using System;
using System.Collections.Generic;
using System.Text;
using Entities;
namespace ServiceContract.DTO
{
    //represents the response DTO for a person, which can be used to send person data back to the client
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }

        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj ==null)
            {
                return false;
            }
            if (obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }
            var other = (PersonResponse)obj;
            return this.PersonID == other.PersonID &&
                   this.PersonName == other.PersonName &&
                   this.Email == other.Email &&
                   this.DateOfBirth == other.DateOfBirth &&
                   this.Gender == other.Gender &&
                   this.CountryId == other.CountryId &&
                   this.Address == other.Address &&
                   this.ReceiveNewsLetters == other.ReceiveNewsLetters &&
                   this.Age == other.Age;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(PersonID, CountryId, DateOfBirth);
        }


    }
    public static class PersonExtensions
    {
        // an extension method that convert an object of person class into PersonResponse class
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth.HasValue) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}
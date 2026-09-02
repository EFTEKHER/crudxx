using System;
using System.Collections.Generic;
using System.Text;
using ServiceContract.DTO.Enums;
using Entities;
namespace ServiceContract.DTO
{

    // acts as a DTO for inserting a new person into the database
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        // Converts the PersonAddRequest DTO to a Person domain model object
        public Person ToPerson()
        {
            return new Person
            {
                PersonID = Guid.NewGuid(),
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender?.ToString(),
                CountryId = this.CountryId,
                Address = this.Address,
                ReceiveNewsLetters = this.ReceiveNewsLetters
            };
        }
    }
}
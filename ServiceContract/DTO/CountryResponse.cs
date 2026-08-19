using System;
using System.Collections.Generic;
using System.Text;
using Entities;
namespace ServiceContract.DTO
/// DTO class that is used to send the country data to the client and to receive the country data from the client. Is it a object Type? 
/// answer: Yes, it is an object type. that is used as return Type for most countryService methods
{
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
    }

    public static class  CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {

            return new CountryResponse
            {
                CountryID = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}

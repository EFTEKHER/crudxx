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

        public override bool Equals(object? obj)
        {
                if(obj == null)
            {
                return false;

            }
                if(obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }
            CountryResponse country_to_compare = (CountryResponse)obj;
            return this.CountryID==country_to_compare.CountryID && this.CountryName==country_to_compare.CountryName;

        }
        public override int GetHashCode()
        {
            return HashCode.Combine(CountryID, CountryName);
        }
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

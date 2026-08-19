using ServiceContract;
using ServiceContract.DTO;
using Entities;
namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {

            // validation : countryname cannot be null or empty and countryAddRequest cannot be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
            }
            //duplicate countriesName are not allowed

            if (_countries.Where(temp=>temp.CountryName==countryAddRequest.CountryName).Count()>0) {
                throw new ArgumentException("Country with the same name already exists.", nameof(countryAddRequest.CountryName));
            
            }



            //convert object from CountryAddRequest to Country Type
            Country country =countryAddRequest.ToCountry();
            country.CountryId = Guid.NewGuid();
            _countries.Add(country);
            return country.ToCountryResponse();




        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(c => c.ToCountryResponse()).ToList();
        }
    }
}

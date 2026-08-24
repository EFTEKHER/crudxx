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
            // generate new guid for countryid and add the country to the list
            country.CountryId = Guid.NewGuid();
            _countries.Add(country);
            return country.ToCountryResponse();




        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? countryId)
        {
            //throw new NotImplementedException();
            if(countryId == null)
            {
                return null;
            }
            

            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryId == countryId);
            return country_response_from_list?.ToCountryResponse();
        }
    }
}

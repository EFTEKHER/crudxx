using ServiceContract.DTO;

namespace ServiceContract
{
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountries();


        //
        CountryResponse? GetCountryById(Guid? countryId);
    }
}

using System;
using System.Collections.Generic;
using ServiceContract;
using Services;
using ServiceContract.DTO;
using Xunit;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            CountryAddRequest? request = null;

            Assert.Throws<ArgumentNullException>(() => { _countriesService.AddCountry(request); });
        }

        [Fact]
        public void AddCountry_NullCountryNameNULL()
        {
            CountryAddRequest? request = new CountryAddRequest { CountryName = null };

            Assert.Throws<ArgumentException>(() => { _countriesService.AddCountry(request); });
        }

        [Fact]
        public void AddCountry_CountryName()
        {
            CountryAddRequest? request = new CountryAddRequest { CountryName = "England" };

            var response = _countriesService.AddCountry(request);

            Assert.NotNull(response);
            Assert.Equal("England", response.CountryName);
        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            CountryAddRequest? request1 = new CountryAddRequest { CountryName = "England" };
            CountryAddRequest? request2 = new CountryAddRequest { CountryName = "England" };

            var response1 = _countriesService.AddCountry(request1);

            Assert.NotNull(response1);
            Assert.Equal("England", response1.CountryName);
            Assert.Throws<ArgumentException>(() => { _countriesService.AddCountry(request2); });
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            List<CountryResponse> response = _countriesService.GetAllCountries();

            Assert.NotNull(response);
            Assert.Empty(response);
        }
        #endregion

        #region GetAllCountriesAddFewCountries
        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            List<CountryResponse> response = new List<CountryResponse>();

            List<CountryAddRequest> requests = new List<CountryAddRequest>
            {
                new CountryAddRequest { CountryName = "England" },
                new CountryAddRequest { CountryName = "Bangladesh" },
                new CountryAddRequest { CountryName = "India" }
            };

            foreach (CountryAddRequest countryRequest in requests)
            {
                response.Add(_countriesService.AddCountry(countryRequest));
            }

            List<CountryResponse> actualResponses = _countriesService.GetAllCountries();
            foreach (CountryResponse countryResponse in response)
            {
                Assert.Contains(actualResponses, item => item.CountryName == countryResponse.CountryName);
            }
        }
        #endregion

        [Fact]
        #region GetCountrywithProperdetails

        public void AddCountry_ProperCountryDetails()
        {
           //arrange
            CountryAddRequest ? request= new CountryAddRequest { CountryName = "Ghana" };
            //act 
            CountryResponse? response = _countriesService.AddCountry(request);
            List<CountryResponse> actualResponses = _countriesService.GetAllCountries();
            //assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, actualResponses);

        }
        #endregion

        [Fact]
        #region countriesByIDnull
        public void GetCountryById_NullId()
        {
            // Arrange
            Guid? countryID = null;

            //Act 
            CountryResponse? country_response_from_get_method = _countriesService.GetCountryById(countryID);
            // Assert

            Assert.Null(country_response_from_get_method);
        }
        #endregion
        
        [Fact]
        #region countriesByIDvalid
        public void GetCountryById_ValidId()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest { CountryName = "Ghana" };
            CountryResponse? addedCountry = _countriesService.AddCountry(request);
            // Act
            CountryResponse? retrievedCountry = _countriesService.GetCountryById(addedCountry.CountryID);
            // Assert
            Assert.NotNull(retrievedCountry);
            Assert.Equal(addedCountry.CountryID, retrievedCountry.CountryID);
            Assert.Equal(addedCountry.CountryName, retrievedCountry.CountryName);
        }
        #endregion
        
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ServiceContract;
using Entities;
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
        public void AddCountry_NullCountry() {
            //arrange
            CountryAddRequest? request = null;
            //Act
           
            //Assert
            Assert.Throws<ArgumentNullException>(() => { _countriesService.AddCountry(request); });
        }

        [Fact]
        public void AddCountry_NullCountryNameNULL()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest { CountryName = null };
            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => { _countriesService.AddCountry(request); });
        }

        //validate with countryName
        [Fact]
        public void AddCountry_CountryName() {

            //arrange
            CountryAddRequest? request = new CountryAddRequest { CountryName = "England" };
            //Act
            var response = _countriesService.AddCountry(request);
            //Assert
            Assert.NotNull(response);
            Assert.Equal("England", response.CountryName);
        }
        [Fact]

        public void AddCountry_DuplicateCountryName()
        {
            //arrange
            CountryAddRequest? request1 = new CountryAddRequest { CountryName = "England " };
            CountryAddRequest? request2 = new CountryAddRequest { CountryName = "Bangladesh" };
            //Act
            var response1 = _countriesService.AddCountry(request1);
            //Assert
            Assert.NotNull(response1);
            Assert.Equal("England", response1.CountryName);
            //Act and Assert for duplicate country name
            Assert.Throws<ArgumentException>(() => { _countriesService.AddCountry(request2); });
        }
        #endregion


        #region GetAllCountries
        [Fact]

        public void GetAllCountries_EmptyList()
        {
            //arrange
            //Act
           List<CountryResponse> response = _countriesService.GetAllCountries();
            //Assert
            Assert.NotNull(response);
            Assert.Empty(response);
        }


        #endregion

        [Fact]
        #region GetAllCountriesAddFewCountriest

        public void GetAllCountries_AddFewCountries()
        {
            List<CountryResponse> response = new List<CountryResponse>();

            //Arrange
            List<CountryAddRequest> requests = new List<CountryAddRequest>
            {
                new CountryAddRequest { CountryName = "England" },
                new CountryAddRequest { CountryName = "Bangladesh" },
                new CountryAddRequest { CountryName = "India" }

            };
            //Act 
            foreach(CountryAddRequest country_Request in requests)
            {
                response.Add(_countriesService.AddCountry(country_Request));
              
            }
            //Assert
            List<CountryResponse> actualResponses = _countriesService.GetAllCountries();
            foreach(CountryResponse countryResponse in response)
            {
                Assert.Contains(countryResponse, actualResponses);
            }
        }

        #endregion


    }
}

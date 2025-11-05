using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUD_Test
{
    public class CountryServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountryServiceTest()
        {
            _countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region Add Country
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //Act
            await _countriesService.AddCountry(countryAddRequest));
        }

        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = null };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act
            await _countriesService.AddCountry(countryAddRequest));
        }

        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsDuplicate()
        {
            //Arrange
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest() { CountryName = "USA" };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(countryAddRequest1);
                await _countriesService.AddCountry(countryAddRequest2);
            });
        }

        //When you supply proper CountryName, it should insert (add) the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "Japan" };

            //Act
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);
            List<CountryResponse> countriesFromGetAllCountries = await _countriesService.GetAllCountries();

            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
            Assert.Contains(countryResponse, countriesFromGetAllCountries);
        }

        #endregion

        #region Get All Country

        //The list of countries should be empty by default (before adding any countries)

        [Fact]
        public async Task GetAllCountry_EmptyList()
        {
            //Act
           List<CountryResponse> countries = await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(countries);
        }

        //The list of countries should be empty by default (before adding any countries)

        [Fact]
        public async Task GetAllCountry_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> countryAddRequest = new List<CountryAddRequest>() { 
            new CountryAddRequest() { CountryName ="USA"},
            new CountryAddRequest() { CountryName ="Japan"},
            new CountryAddRequest() { CountryName ="China"},
            };

            //Act
            List<CountryResponse> countryListFromAddCountry = new List<CountryResponse>();

            foreach (CountryAddRequest country in countryAddRequest)
            {
                countryListFromAddCountry.Add(await _countriesService.AddCountry(country));
            }

            List<CountryResponse> actualCountryResponseList =await _countriesService.GetAllCountries();

            foreach(CountryResponse expectedCountry in countryListFromAddCountry) 
            {
                //Assert
                Assert.Contains(expectedCountry, actualCountryResponseList);
            }
        }

        #endregion

        #region Get Country By CountryID

        //if we supply null as CountryID, it should return null as CountryResponse 
        [Fact]
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;

            //Act
            CountryResponse countryIDByCountryResponse = await _countriesService.GetCountryByCountryID(countryID);

            //Assert
            Assert.Null(countryIDByCountryResponse);
        }

        //if we supply valid country id, it should return the matching country details as CountryResponse object
        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "China" };

            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            //Act
            CountryResponse countryIDFromCountryResponse = await _countriesService.GetCountryByCountryID(countryResponseFromAdd.CountryID);

            //Assert
            Assert.Equal(countryResponseFromAdd, countryIDFromCountryResponse);
        }

        #endregion
    }
}
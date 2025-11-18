using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUD_Test
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);
            _countriesService = new CountriesService(dbContext);
            _personService = new PersonsService(dbContext, _countriesService);
            _testOutputHelper = testOutputHelper;

            _fixture = new Fixture();
        }

        #region AddPerson

        // When we supply null value as PersonAddRequest it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.AddPerson(personAddRequest);
            });
        }

        // When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse which includes with the newly generated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com").Create();

            //Act
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);

            List<PersonResponse> personsList = await _personService.GetAllPersons();

            //Assert
            Assert.True(personResponseFromAdd.PersonID != Guid.Empty);
            Assert.Contains(personResponseFromAdd, personsList);
        }
        #endregion

        #region GetPersonByPersonID

        //if we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            Guid? personID = null;

            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonID(personID);

            Assert.Null(personResponseFromGet);
        }

        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            //Act
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com")
                .With(temp => temp.CountryID, countryResponseFromAdd.CountryID).Create();

            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);

            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonID(personResponseFromAdd.PersonID);

            //Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }

        #endregion

        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_ListIsEmpty()
        {
            //Act
            List<PersonResponse> PersonsFromGet = await _personService.GetAllPersons();

            //Assert
            Assert.Empty(PersonsFromGet);
        }

        //First, we will add few persons; and then when we calll GetAllPersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest countryCodeAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryCodeAddRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryCodeAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryCodeAddRequest2);

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone1@example.com")
                .With(temp => temp.CountryID, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone2@example.com")
                .With(temp => temp.CountryID, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone3@example.com")
                .With(temp => temp.CountryID, countryResponse2.CountryID).Create();

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2, personAddRequest3
            };

            List<PersonResponse> personsResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest request in personAddRequests)
            {
                PersonResponse addedPersons = await _personService.AddPerson(request);

                personsResponseListFromAdd.Add(addedPersons);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsResponseListFromGet = await _personService.GetAllPersons();

            //print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponseFromGet in personsResponseListFromGet)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            //Assert
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                Assert.Contains(personResponseFromAdd, personsResponseListFromGet);
            }

        }

        #endregion

        #region GetFilteredPersons

        //if the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest countryCodeAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryCodeAddRequest2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryCodeAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryCodeAddRequest2);

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone1@example.com").Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone2@example.com").Create();
            PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone3@example.com").Create();

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2, personAddRequest3
            };

            List<PersonResponse> personsResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest request in personAddRequests)
            {
                PersonResponse addedPersons = await _personService.AddPerson(request);

                personsResponseListFromAdd.Add(addedPersons);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsResponseListFromSearch = await _personService.GetFilteredPersons(nameof(PersonResponse.PersonName), "");

            //print person_response_list_from_search
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponseFromGet in personsResponseListFromSearch)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            //Assert
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                Assert.Contains(personResponseFromAdd, personsResponseListFromSearch);
            }
        }

        //first we will add few persons, and then we will search based on person name with some search string. it should return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest countryCodeAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryCodeAddRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryCodeAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryCodeAddRequest2);

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Aman")
                .With(temp => temp.Email, "someone1@example.com")
                .With(temp => temp.CountryID, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Maria")
                .With(temp => temp.Email, "someone2@example.com")
                .With(temp => temp.CountryID, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Kamalika")
                .With(temp => temp.Email, "someone3@example.com")
                .With(temp => temp.CountryID, countryResponse2.CountryID).Create();

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2, personAddRequest3
            };

            List<PersonResponse> personsResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest request in personAddRequests)
            {
                PersonResponse addedPersons = await _personService.AddPerson(request);

                personsResponseListFromAdd.Add(addedPersons);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsResponseListFromSearch = await _personService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ma");

            //print person_response_list_from_search
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponseFromGet in personsResponseListFromSearch)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            //Assert
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                if (personResponseFromAdd.PersonName != null)
                {
                    if (personResponseFromAdd.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponseFromAdd, personsResponseListFromSearch);
                    }
                }
            }
        }

        #endregion

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public async Task GetSortedPersons_()
        {
            //Arrange
            CountryAddRequest countryCodeAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryCodeAddRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryCodeAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryCodeAddRequest2);

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone1@example.com")
                .With(temp => temp.CountryID, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone1@example.com")
                .With(temp => temp.CountryID, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone2@example.com")
                .With(temp => temp.CountryID, countryResponse1.CountryID).Create();

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2, personAddRequest3
            };

            List<PersonResponse> personsResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest request in personAddRequests)
            {
                PersonResponse addedPersons = await _personService.AddPerson(request);

                personsResponseListFromAdd.Add(addedPersons);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            List<PersonResponse> allPersons = await _personService.GetAllPersons();

            //Act
            List<PersonResponse> personsResponseListFromSort = await _personService.GetSortedPersons(allPersons, nameof(PersonResponse.PersonName), SortOrderOptions.DESC);

            //print person_response_list_from_search
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponseFromGet in personsResponseListFromSort)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            personsResponseListFromAdd = personsResponseListFromAdd.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert
            for (int i = 0; i < personsResponseListFromAdd
                 .Count; i++)
            {
                Assert.Equal(personsResponseListFromAdd[i], personsResponseListFromSort[i]);
            }
        }

        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>

            //Act
            await _personService.UpdatePerson(personUpdateRequest));
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = _fixture.Create<PersonUpdateRequest>();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>

            //Act
            await _personService.UpdatePerson(personUpdateRequest));
        }

        //When personName, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "David")
                .With(temp => temp.Email, "david@example.com")
                .With(temp => temp.CountryID, countryResponseFromAdd.CountryID).Create();

            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>

            //Act
            await _personService.UpdatePerson(personUpdateRequest));
        }

        //First, add a new person and try to update the persome name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "David")
                .With(temp => temp.Email, "david@example.com")
                .With(temp => temp.CountryID, countryResponseFromAdd.CountryID).Create();

            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "John";
            personUpdateRequest.Email = "johncena@email.com";

            //Act
            PersonResponse personResponseFromUpdate = await _personService.UpdatePerson(personUpdateRequest);

            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonID(personResponseFromUpdate.PersonID) ;

            //Assert
            Assert.Equal(personResponseFromGet, personResponseFromUpdate);
        }

        #endregion

        #region DeletePerson

        //If you supply an valid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "David")
                .With(temp => temp.Email, "david@example.com")
                .With(temp => temp.CountryID, countryResponseFromAdd.CountryID).Create();

            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);

            //Act
            bool isDeleted =  await _personService.DeletePerson(personResponseFromAdd.PersonID);
            
            //Assert
            Assert.True(isDeleted);
        }

        //If you supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());
            
            //Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}

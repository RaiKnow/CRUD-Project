using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUD_Test
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;

        public PersonsServiceTest()
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
        }

        #region AddPerson

        // When we supply null value as PersonAddRequest it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        // When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse which includes with the newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Francisco Brocklehurst", Email = "fbrocklehurst0@wp.com", DateOfBirth = DateTime.Parse("2018/05/02"), Gender = GenderOptions.Male, CountryID = Guid.NewGuid(), Address = "Apt 154", ReceiveNewsLetters = true };

            //Act
            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            List<PersonResponse> personsList = _personService.GetAllPersons();

            //Assert
            Assert.True(personResponseFromAdd.PersonID != Guid.Empty);
            Assert.Contains(personResponseFromAdd, personsList);
        }
        #endregion

        #region GetPersonByPersonID

        //if we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            Guid? personID = null;

            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonID(personID);

            Assert.Null(personResponseFromGet);
        }

        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            CountryResponse countryResponseFromAdd = _countriesService.AddCountry(countryAddRequest);

            //Act
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Francisco Brocklehurst", Email = "fbrocklehurst0@wp.com", DateOfBirth = DateTime.Parse("2018/05/02"), Gender = GenderOptions.Male, CountryID = countryResponseFromAdd.CountryID, Address = "Apt 154", ReceiveNewsLetters = true };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonID(personResponseFromAdd.PersonID);

            //Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }

        #endregion

        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public void GetAllPersons_ListIsEmpty()
        {
            //Act
            List<PersonResponse> PersonsFromGet = _personService.GetAllPersons();

            //Assert
            Assert.Empty(PersonsFromGet);
        }

        //First, we will add few persons; and then when we calll GetAllPersons(), it should return the same persons that were added
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest countryCodeAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryCodeAddRequest2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(countryCodeAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryCodeAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "John Smith",
                Email = "johnsmith@gmail.com",
                DateOfBirth = DateTime.Parse("2018/05/02"),
                Gender = GenderOptions.Male,
                CountryID = countryResponse1.CountryID,
                Address = "Apt 154",
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Adam Sandler",
                Email = "adamsandler@gmail.com",
                DateOfBirth = DateTime.Parse("2006/03/09"),
                Gender = GenderOptions.Male,
                CountryID = countryResponse2.CountryID,
                Address = "St. Abu 798",
                ReceiveNewsLetters = false
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Julies Cruise",
                Email = "juliescruise@gmail.com",
                DateOfBirth = DateTime.Parse("2009/06/08"),
                Gender = GenderOptions.Female,
                CountryID = countryResponse1.CountryID,
                Address = "Los Santos",
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2, personAddRequest3
            };

            List<PersonResponse> personsResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest request in personAddRequests)
            {
                PersonResponse addedPersons = _personService.AddPerson(request);

                personsResponseListFromAdd.Add(addedPersons);
            }

            //Act
            List<PersonResponse> personsResponseFromGet = _personService.GetAllPersons();

            //Assert
            foreach(PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                Assert.Contains(personResponseFromAdd, personsResponseFromGet);
            }
            
        }

        #endregion
    }
}

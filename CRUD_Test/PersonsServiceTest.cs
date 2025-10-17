﻿using ServiceContracts;
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

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService(false);
            _countriesService = new CountriesService(false);
            _testOutputHelper = testOutputHelper;
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

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsResponseListFromGet = _personService.GetAllPersons();

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
        public void GetFilteredPersons_EmptySearchText()
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

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsResponseListFromSearch = _personService.GetFilteredPersons(nameof(PersonResponse.PersonName), "");

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
        public void GetFilteredPersons_SearchByPersonName()
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

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsResponseListFromSearch = _personService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ma");

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
        public void GetSortedPersons_()
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

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personsResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            List<PersonResponse> allPersons = _personService.GetAllPersons();

            //Act
            List<PersonResponse> personsResponseListFromSort = _personService.GetSortedPersons(allPersons, nameof(PersonResponse.PersonName), SortOrderOptions.DESC);

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
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>

            //Act
            _personService.UpdatePerson(personUpdateRequest));
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest { PersonID = Guid.NewGuid() };

            //Assert
            Assert.Throws<ArgumentException>(() =>

            //Act
            _personService.UpdatePerson(personUpdateRequest));
        }

        //When personName, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse countryResponseFromAdd = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Francisco Brocklehurst",
                Email = "fbrocklehurst0@wp.com",
                DateOfBirth = DateTime.Parse("2018/05/02"),
                Gender = GenderOptions.Male,
                CountryID = countryResponseFromAdd.CountryID,
                Address = "Apt 154",
                ReceiveNewsLetters = true
            };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = null;

            //Assert
            Assert.Throws<ArgumentException>(() =>

            //Act
            _personService.UpdatePerson(personUpdateRequest));
        }

        //First, add a new person and try to update the persome name and email
        [Fact]
        public void UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse countryResponseFromAdd = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Francisco Brocklehurst",
                Email = "fbrocklehurst0@wp.com",
                DateOfBirth = DateTime.Parse("2018/05/02"),
                Gender = GenderOptions.Male,
                CountryID = countryResponseFromAdd.CountryID,
                Address = "Apt 154",
                ReceiveNewsLetters = true
            };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "John";
            personUpdateRequest.Email = "johncena@email.com";

            //Act
            PersonResponse personResponseFromUpdate = _personService.UpdatePerson(personUpdateRequest);

            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonID(personResponseFromUpdate.PersonID) ;

            //Assert
            Assert.Equal(personResponseFromGet, personResponseFromUpdate);
        }

        #endregion

        #region DeletePerson

        //If you supply an valid PersonID, it should return false
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse countryResponseFromAdd = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Francisco Brocklehurst",
                Email = "fbrocklehurst0@wp.com",
                DateOfBirth = DateTime.Parse("2018/05/02"),
                Gender = GenderOptions.Male,
                CountryID = countryResponseFromAdd.CountryID,
                Address = "Apt 154",
                ReceiveNewsLetters = true
            };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            //Act
            bool isDeleted =  _personService.DeletePerson(personResponseFromAdd.PersonID);
            
            //Assert
            Assert.True(isDeleted);
        }

        //If you supply an invalid PersonID, it should return false
        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted =  _personService.DeletePerson(Guid.NewGuid());
            
            //Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}

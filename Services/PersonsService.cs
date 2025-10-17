using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            _persons.AddRange(new List<Person>() {
            new Person()
            {
                PersonID = Guid.Parse("2F4322B9-D231-4D6C-89DA-EB0821467FBB"), PersonName = "Tory Oventon", Email = "toventon0@ucla.edu", DateOfBirth = DateTime.Parse("11/09/2009"), Gender = GenderOptions.Female.ToString(), Address = "Apt 841", ReceiveNewsLetters = true, CountryID = Guid.Parse("0C5B9025-A178-422F-9772-B83CF42F3FBA")
            },
            new Person()
            {
                PersonID = Guid.Parse("A116271C-7443-4323-84C5-D41BA2D96BE9"), PersonName = "Rosy Hessay", Email = "rhessay1@jalbum.net", DateOfBirth = DateTime.Parse("24/12/1997"), Gender = GenderOptions.Female.ToString(), Address = "6th Floor", ReceiveNewsLetters = false, CountryID = Guid.Parse("148FECC0-F5AA-43A1-9744-1DA4289C7A5A")
            },
            new Person()
            {
                PersonID = Guid.Parse("3B372230-D9B4-4952-99F4-3E9369FA0DB4"), PersonName = "Holly Fishlee", Email = "hfishlee2@ucoz.com", DateOfBirth = DateTime.Parse("14/11/1992"), Gender = GenderOptions.Male.ToString(), Address = "PO Box 5286", ReceiveNewsLetters = true, CountryID = Guid.Parse("F7F1E057-CC8D-4C34-B03F-32387397DAA2")
            },
            new Person()
            {
                PersonID = Guid.Parse("B0DA6468-792D-4649-B1DD-62D6F9C22735"), PersonName = "Teresita Palleske", Email = "tpalleske3@weibo.com", DateOfBirth = DateTime.Parse("18/12/2015"), Gender = GenderOptions.Other.ToString(), Address = "Room 861", ReceiveNewsLetters = true, CountryID = Guid.Parse("206418E0-F767-4689-BAC9-B0AA7696E60A")
            },
            new Person()
            {
                PersonID = Guid.Parse("B4E88AFA-BA23-4861-BC66-2DEEBC899239"), PersonName = "Brana Yarrow", Email = "byarrow4@utexas.edu", DateOfBirth = DateTime.Parse("15/09/2025"), Gender = GenderOptions.Female.ToString(), Address = "2nd Floor", ReceiveNewsLetters = true, CountryID = Guid.Parse("F7F1E057-CC8D-4C34-B03F-32387397DAA2")
            },
            new Person()
            {
                PersonID = Guid.Parse("3CD4AD95-B7D4-43A9-B1A8-C09A3FFE8612"), PersonName = "Nicola McCullen", Email = "nmccullen5@national.com", DateOfBirth = DateTime.Parse("17/04/2024"), Gender = GenderOptions.Female.ToString(), Address = "Suite 59", ReceiveNewsLetters = false, CountryID = Guid.Parse("206418E0-F767-4689-BAC9-B0AA7696E60A")
            },
            new Person()
            {
                PersonID = Guid.Parse("0ECABF09-8D29-4CAB-9593-FFDA5C1016F0"), PersonName = "Peyter Harman", Email = "pharman4@github.com", DateOfBirth = DateTime.Parse("02/08/1995"), Gender = GenderOptions.Male.ToString(), Address = "PO Box 53534", ReceiveNewsLetters = false, CountryID = Guid.Parse("148FECC0-F5AA-43A1-9744-1DA4289C7A5A")
            },
            new Person()
            {
                PersonID = Guid.Parse("1884CF5F-9CAC-4D2E-8407-AE688F39ADC8"), PersonName = "Erroll Thoma", Email = "ethoma6@mozilla.com", DateOfBirth = DateTime.Parse("17/07/2006"), Gender = GenderOptions.Male.ToString(), Address = "Room 1638", ReceiveNewsLetters = false, CountryID = Guid.Parse("206418E0-F767-4689-BAC9-B0AA7696E60A")
            },
            new Person()
            {
                PersonID = Guid.Parse("B32C879F-C578-4F6F-B6A6-DE5069AE2F4B"), PersonName = "Serge Ewebank", Email = "sewebank7@blogs.com", DateOfBirth = DateTime.Parse("03/10/2003"), Gender = GenderOptions.Male.ToString(), Address = "Apt 1481", ReceiveNewsLetters = true, CountryID = Guid.Parse("0C5B9025-A178-422F-9772-B83CF42F3FBA")
            },
            new Person()
            {
                PersonID = Guid.Parse("47F724A7-7851-46F5-8B1C-C3F3B9D29EB9"), PersonName = "Boothe Chiverton", Email = "bchiverton2@ucsd.edu", DateOfBirth = DateTime.Parse("06/04/1990"), Gender = GenderOptions.Other.ToString(), Address = "5th Floor", ReceiveNewsLetters = true, CountryID = Guid.Parse("B7D4A259-C1DF-42A1-B9CB-1D833D1580A3")
            },
            });

        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            //convert Person to PersonResponse type
            PersonResponse newPersonResponse = person.ToPersonResponse();

            newPersonResponse.Country = _countriesService.GetCountryByCountryID(newPersonResponse.CountryID)?.CountryName;

            return newPersonResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));

            //Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            //convert personAddRequest into Person type
            Person newPerson = personAddRequest.ToPerson();

            //generate PersonID
            newPerson.PersonID = Guid.NewGuid();

            //add person object to persons list
            _persons.Add(newPerson);

            return ConvertPersonToPersonResponse(newPerson);
        }

        public List<PersonResponse> GetAllPersons()
        {
            List<PersonResponse> allPersons = _persons.Select(temp => ConvertPersonToPersonResponse(temp)).ToList();

            return allPersons;
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if(personID == null)
                return null;

            Person? existingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personID);

            if(existingPerson == null)
                return null;

            PersonResponse existingPersonResponse = ConvertPersonToPersonResponse(existingPerson);

            return existingPersonResponse;
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return matchingPersons;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName)? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase): true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(temp => (temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString("yyyy-mm-dd").Contains(searchString) : true).ToList();
                    break;
                
                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                
                case nameof(PersonResponse.CountryID):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                
                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {

                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null)
                throw new ArgumentNullException(nameof(Person));

            //Model validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);

            if(matchingPerson == null)
                throw new ArgumentException("Given person id doesn't exist");

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.CountryID = personUpdateRequest.CountryID;

            return ConvertPersonToPersonResponse(matchingPerson);
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null)
                throw new ArgumentNullException(nameof(personID));

            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);

            if (person == null)
                return false;

            bool isDeleted = _persons.Remove(person);

            return isDeleted;
        }
    }
}

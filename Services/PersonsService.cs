using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
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
            List<PersonResponse> allPersons = _persons.Select(temp => temp.ToPersonResponse()).ToList();

            return allPersons;
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if(personID == null)
                return null;

            Person? existingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personID);

            if(existingPerson == null)
                return null;

            PersonResponse existingPersonResponse = existingPerson.ToPersonResponse();

            return existingPersonResponse;
        }
    }
}

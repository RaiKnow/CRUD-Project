using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDbContext _db;

        public CountriesService(PersonsDbContext personsDbContext)
        {
            _db = personsDbContext;
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (_db.Countries.Count(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
                throw new ArgumentException("Given country name is already exists");

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse GetCountryByCountryID(Guid? CountryID)
        {
            if (CountryID == null)
                return null;

            Country? country = _db.Countries.FirstOrDefault(temp => temp.CountryID == CountryID);

            if (country == null)
                return null;

            return country.ToCountryResponse();
        }
    }
}

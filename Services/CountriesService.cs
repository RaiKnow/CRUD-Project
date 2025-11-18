using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ApplicationDbContext _db;

        public CountriesService(ApplicationDbContext personsDbContext)
        {
            _db = personsDbContext;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (_db.Countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
                throw new ArgumentException("Given country name is already exists");

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse> GetCountryByCountryID(Guid? CountryID)
        {
            if (CountryID == null)
                return null;

            Country? country = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == CountryID);

            if (country == null)
                return null;

            return country.ToCountryResponse();
        }
    }
}

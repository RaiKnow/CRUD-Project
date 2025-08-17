using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null) 
                throw new ArgumentNullException(nameof(countryAddRequest));

            if(string.IsNullOrEmpty(countryAddRequest.CountryName))
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
                throw new ArgumentException("Given country name is already exists");

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse GetCountryByCountryID(Guid? CountryID)
        {
            if (CountryID == null)
                return null;
            
            Country? country = _countries.FirstOrDefault(temp => temp.CountryID == CountryID);

            if (country == null)
                return null;

            return country.ToCountryResponse();
        }
    }
}

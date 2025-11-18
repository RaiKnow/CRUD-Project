using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();

            if (initialize)
            {
                _countries.AddRange(new List<Country>() {
                new Country()
                {
                    CountryID = Guid.Parse("0C5B9025-A178-422F-9772-B83CF42F3FBA"), CountryName = "USA"
                },

                new Country()
                {
                    CountryID = Guid.Parse("206418E0-F767-4689-BAC9-B0AA7696E60A"), CountryName = "Canada"
                },
                
                new Country()
                {
                    CountryID = Guid.Parse("B7D4A259-C1DF-42A1-B9CB-1D833D1580A3"), CountryName = "UK"
                },

                new Country()
                {
                    CountryID = Guid.Parse("F7F1E057-CC8D-4C34-B03F-32387397DAA2"), CountryName = "India"
                },

                new Country()
                {
                    CountryID = Guid.Parse("148FECC0-F5AA-43A1-9744-1DA4289C7A5A"), CountryName = "Australia"
                },
                });
            }
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
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

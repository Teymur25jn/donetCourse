using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Response;

namespace Services;

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
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        if (string.IsNullOrWhiteSpace(countryAddRequest.CountryName))
        {
            throw new ArgumentException("Country name cannot be null or whitespace.", nameof(countryAddRequest.CountryName));
        }

        if (_countries.Any(c=> c.CountryName == countryAddRequest.CountryName))
        {
            throw new ArgumentException("Given Country name already exists.");
        }
        
        Country country = countryAddRequest.ToCountry();
        
        country.CountryId = Guid.NewGuid();
        
        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
       return _countries.Select(c => c.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryId(Guid? countryId)
    {
        if (countryId is null)
            return null;
        Country? countryForGivenId = _countries.FirstOrDefault(c => c.CountryId == countryId);

        if (countryForGivenId is null)
            return null;
        
        return countryForGivenId.ToCountryResponse();
    }
}
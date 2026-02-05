using System;
using System.Collections.Generic;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Response;

namespace ServiceContracts;

public interface ICountriesService
{
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

    public List<CountryResponse> GetAllCountries();
    
    public CountryResponse? GetCountryByCountryId(Guid? countryId);
}
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Response;
using Services;

namespace CRUDExampleTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        this._countriesService = new CountriesService();
    }
    
    #region AddCountry
    //When the CountryAddRequest is null , it should throw ArgumentNullException
    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? countryAddRequest = null;
        
        //Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //Act
            _countriesService.AddCountry(countryAddRequest);
        });
    }
    
    //When CountryName is null , it should throw ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        //Arrange
        CountryAddRequest? countryAddRequest = new CountryAddRequest()
        {
            CountryName = null
        };
        
        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _countriesService.AddCountry(countryAddRequest);
        });
    }
    
    //When the CountryName is duplicate , it should throw ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        //Arrange
        CountryAddRequest? countryAddRequest1 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        
        CountryAddRequest? countryAddRequest2 = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        
        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _countriesService.AddCountry(countryAddRequest1);
            _countriesService.AddCountry(countryAddRequest2);
        });
    }
    
    [Fact]
    public void AddCountry_ProperCountryName()
    {
        //Arrange
        CountryAddRequest? countryAddRequest = new CountryAddRequest()
        {
            CountryName = "USA"
        };
        
        //Act
        CountryResponse response = _countriesService.AddCountry(countryAddRequest);
        
        List<CountryResponse> countries_from_getAllCountries = _countriesService.GetAllCountries();
        //Assert
        Assert.True(response.CountryId != Guid.Empty);
        
        Assert.Contains(response , countries_from_getAllCountries);
    }
    #endregion
    
    #region GetAllCountries
    [Fact]
    public void GetAllCountries_EmptyList()
    {
        //Arrange 
        //Acts 
        List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();
        
        //Assert 
        Assert.Empty(actual_country_response_list);
    }

    [Fact]
    public void GetAllCountries_AddFewCountries()
    {
        //Arrange
        List<CountryAddRequest> country_add_request_list = new List<CountryAddRequest>()
        {
            new CountryAddRequest()
            {
                CountryName = "USA"
            },
            new CountryAddRequest()
            {
                CountryName = "UK"
            }
        };

        List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
        
        //Act
        foreach (CountryAddRequest country_add_request in country_add_request_list)
        {
            countries_list_from_add_country.Add(_countriesService.AddCountry(country_add_request));
        }
        
        List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

        foreach (CountryResponse expected_country in countries_list_from_add_country)
        {
            Assert.Contains(expected_country , actual_country_response_list);
        }
    }
    #endregion
    
    #region GetCountryById
    [Fact]
    public void GetCountryById_NullCountryId()
    {
        //Arrange 
        Guid? countryId = null;
        
        //Act 
        CountryResponse? response = _countriesService.GetCountryByCountryId(countryId);
        
        //Assert 
        Assert.Null(response);
    }

    [Fact]
    public void GetCountryByCountryId_ValidCountryId()
    {
        //Arrange 
        CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "China" };
        CountryResponse? responseFromAddCountry = _countriesService.AddCountry(countryAddRequest);
        
        //Act 
        CountryResponse? responseFromGetCountry = _countriesService.GetCountryByCountryId(responseFromAddCountry.CountryId);
        
        //Assert 
        Assert.Equal(responseFromAddCountry,responseFromGetCountry);
    }
    #endregion
}
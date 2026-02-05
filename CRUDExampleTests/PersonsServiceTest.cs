using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Extensions;
using Extensions.ENUMs;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Request;
using ServiceContracts.DTO.Response;
using Services;
using Xunit;
using Xunit.Abstractions;

namespace CRUDExampleTests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;
    
    private readonly ICountriesService _countriesService;
    
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _countriesService = new CountriesService();
        _personsService = new PersonsService();
    }

    #region AddPerson
    [Fact]
    public void AddPerson_NullPerson()
    {
        //Arrange
        PersonAddRequest? personAddRequest = null;

        Assert.Throws<ArgumentNullException>(() =>
        {
            _personsService.AddPerson(personAddRequest);
        });
    }

    [Fact]
    public void AddPerson_PersonNameIsNull()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = null
        };

        Assert.Throws<ArgumentException>(() =>
        {
            _personsService.AddPerson(personAddRequest);
        });
    }
    
    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "Person Name",
            Email = "person@example.com",
            Address = "Person Address",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true,
            DateOfBirth = DateTime.Now.AddYears(-22)
        };

        //Act
        PersonResponse  personResponseFromAddPerson = _personsService.AddPerson(personAddRequest);
        
        var actualPersonsFromGetAllPersons = _personsService.GetAllPersons();

        //Assert
        Assert.True(personResponseFromAddPerson.PersonId != Guid.Empty);
        
        Assert.Contains(personResponseFromAddPerson, actualPersonsFromGetAllPersons);
        
    }
    #endregion
    
    #region GetPersonByPersonId
    [Fact]
    public void GetPersonByPersonId_NullPersonId()
    {
        //Arrange 
        var personId = Guid.Empty;
        
        //Act 
        var personResponseFromGetPersonById = _personsService.GetPersonByPersonId(personId);
        
        //Assert 
        Assert.Null(personResponseFromGetPersonById);
    }

    [Fact]
    public void GetPersonByPersonId_WithPersonId()
    {
        //Arrange
        CountryResponse? countryResponse = _countriesService.AddCountry(new CountryAddRequest()
        {
            CountryName = "Bangladesh"
        });

        //Act
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "Person Name",
            Email = "test@example.com",
            Address = "Person Address",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true,
            DateOfBirth = DateTime.Now.AddYears(-25),
            CountryId = countryResponse.CountryId
        };
        
        PersonResponse? personResponse = _personsService.AddPerson(personAddRequest);
        
        PersonResponse? personResponseFromGetPersonById = _personsService.GetPersonByPersonId(personResponse.PersonId);
        
        //Assert
        Assert.Equal(personResponseFromGetPersonById, personResponse);
    }
    #endregion
    
    #region GetAllPersons
    [Fact]
    public void GetAllPersons_EmptyList()
    {
        //Act
        var personResponse = _personsService.GetAllPersons();
        
        _testOutputHelper.WriteLine("first person: " + personResponse.FirstOrDefault()?.PersonName);
        
        //Assert
        Assert.Empty(personResponse);
    }

    [Fact]
    public void GetAllPersons_AddFewPersons()
    {
        //Arrange 
        CountryAddRequest? countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Bangladesh"
        };
        
        CountryResponse? countryResponse = _countriesService.AddCountry(countryAddRequest);
        
        List<PersonAddRequest?> personAddRequests = new List<PersonAddRequest?>()
        {
            new  PersonAddRequest()
            {
                PersonName = "Person Name1",
                Email = "sample@gmail.com",
                Address = "Person Address1",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Now.AddYears(-25),
                CountryId = countryResponse.CountryId
            },
            new  PersonAddRequest()
            {
                PersonName = "Person Name2",
                Email = "sample2@gmail.com",
                Address = "Person Address2",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Now.AddYears(-50),
                CountryId = countryResponse.CountryId
            }
        };

        List<PersonResponse?> personResponses = new List<PersonResponse?>();
        foreach (var personAddRequest in personAddRequests)
        {
            personResponses.Add(_personsService.AddPerson(personAddRequest)); 
        }
        
        //Act 
        List<PersonResponse?>  personResponseFromGetAllPersons = _personsService.GetAllPersons();
        
        //Assert 
        foreach (var personResponse in personResponses)
        {
            Assert.Contains(personResponse,personResponseFromGetAllPersons);
        }
    }
    #endregion

    #region GetFilteredPersons
    [Fact]
    public void GetFilteredPersons_EmptySearchText()
    {
        //Arrange 
        CountryAddRequest? countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Bangladesh"
        };
        
        CountryResponse? countryResponse = _countriesService.AddCountry(countryAddRequest);
        
        List<PersonAddRequest?> personAddRequests = new List<PersonAddRequest?>()
        {
            new  PersonAddRequest()
            {
                PersonName = "Person Name1",
                Email = "sample@gmail.com",
                Address = "Person Address1",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Now.AddYears(-25),
                CountryId = countryResponse.CountryId
            },
            new  PersonAddRequest()
            {
                PersonName = "Person Name2",
                Email = "sample2@gmail.com",
                Address = "Person Address2",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Now.AddYears(-50),
                CountryId = countryResponse.CountryId
            }
        };

        List<PersonResponse?> personResponses = new List<PersonResponse?>();
        foreach (var personAddRequest in personAddRequests)
        {
            personResponses.Add(_personsService.AddPerson(personAddRequest)); 
        }
        
        //Act 
        List<PersonResponse?> personResponseFromGetFilteredPersons = _personsService.GetFilteredPersons(nameof(Person.PersonName) , "");
        
        //Assert 
        foreach (var personResponse in personResponses)
        {
            Assert.Contains(personResponse,personResponseFromGetFilteredPersons);
        }
    }
    
    [Fact]
    public void GetFilteredPersons_SearchByPersonName()
    {
        //Arrange 
        CountryAddRequest? countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Bangladesh"
        };
        
        CountryResponse? countryResponse = _countriesService.AddCountry(countryAddRequest);
        
        List<PersonAddRequest?> personAddRequests = new List<PersonAddRequest?>()
        {
            new  PersonAddRequest()
            {
                PersonName = "Person Name1",
                Email = "sample@gmail.com",
                Address = "Person Address1",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Now.AddYears(-25),
                CountryId = countryResponse.CountryId
            },
            new  PersonAddRequest()
            {
                PersonName = "Person Name2",
                Email = "sample2@gmail.com",
                Address = "Person Address2",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Now.AddYears(-50),
                CountryId = countryResponse.CountryId
            }
        };

        List<PersonResponse?> personResponses = new List<PersonResponse?>();
        foreach (var personAddRequest in personAddRequests)
        {
            personResponses.Add(_personsService.AddPerson(personAddRequest)); 
        }
        
        //Act 
        List<PersonResponse?> personResponseFromGetFilteredPersons = _personsService.GetFilteredPersons(nameof(Person.PersonName) , "ma");
        
        //Assert 
        foreach (var personResponse in personResponses)
        {
            if (personResponse?.PersonName is not null)
            {
                if (personResponse.PersonName.Contains("ma",StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(personResponse,personResponseFromGetFilteredPersons);
                }
            }
        }
    }
    #endregion
    
    #region GetSortedPersons

    [Fact]
    public void GetSortedPersons_EmptySearchText()
    {
        //Arrange 
        CountryAddRequest? countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Bangladesh"
        };
        
        CountryResponse? countryResponse = _countriesService.AddCountry(countryAddRequest);
        
        List<PersonAddRequest?> personAddRequests = new List<PersonAddRequest?>()
        {
            new  PersonAddRequest()
            {
                PersonName = "Person Name1",
                Email = "sample@gmail.com",
                Address = "Person Address1",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Now.AddYears(-25),
                CountryId = countryResponse.CountryId
            },
            new  PersonAddRequest()
            {
                PersonName = "Person Name2",
                Email = "sample2@gmail.com",
                Address = "Person Address2",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Now.AddYears(-50),
                CountryId = countryResponse.CountryId
            }
        };

        List<PersonResponse?> personResponses = new List<PersonResponse?>();
        foreach (var personAddRequest in personAddRequests)
        {
            personResponses.Add(_personsService.AddPerson(personAddRequest)); 
        }
        
        var allPersons = _personsService.GetAllPersons();
        
        //Act 
        List<PersonResponse?> personResponseFromGetSortedPersons = _personsService.GetSortedPersons( allPersons , nameof(Person.PersonName), SortOrderOptions.Desc);
        
        //Assert 
        foreach (var personResponse in personResponses)
        {
            if (personResponse?.PersonName is not null)
            {
                if (personResponse.PersonName.Contains("ma",StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(personResponse,personResponseFromGetSortedPersons);
                }
            }
        }
    }
    #endregion
    
    #region UpdatePerson

    [Fact]
    public void UpdatePerson_NullPerson()
    {
        //Arrange 
        PersonUpdateRequest? personUpdateRequest = null;
        
        //Act
        Assert.Throws<ArgumentNullException>(() =>
        {
            _personsService.UpdatePerson(personUpdateRequest);
        });
    }

    [Fact]
    public void UpdatePerson_InvalidPersonId()
    {
        //Arrange 
        PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
        {
            PersonId = Guid.NewGuid()
        };
        
        //Act
        Assert.Throws<ArgumentException>(() =>
        {
            _personsService.UpdatePerson(personUpdateRequest);
        });
    }
    #endregion

    #region DeletePerson
    [Fact]
    public void DeletePerson_ValidPersonId()
    {
        CountryAddRequest? countryAddRequest = new CountryAddRequest()
        {
            CountryName = "Bangladesh"
        };
        
        CountryResponse? countryResponse = _countriesService.AddCountry(countryAddRequest);

        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "faaaah",
            Address = "Person Address1",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true,
            DateOfBirth = DateTime.Now.AddYears(-25),
            Email = "test@gmail.com",
            CountryId = countryResponse.CountryId
        };

        PersonResponse? personResponse = _personsService.AddPerson(personAddRequest);
        
        bool isDeleted = _personsService.DeletePerson(personResponse.PersonId);
        Assert.True(isDeleted);
        
    }
    
    [Fact]
    public void DeletePerson_InvalidPersonId()
    {
        bool isDeleted = _personsService.DeletePerson(Guid.NewGuid());
        Assert.False(isDeleted);
    }
    #endregion
}
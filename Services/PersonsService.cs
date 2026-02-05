using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entities;
using Extensions.ENUMs;
using ServiceContracts;
using ServiceContracts.DTO.Request;
using ServiceContracts.DTO.Response;
using Services.Helpers;
using Xunit.Abstractions;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly List<Person> _people;
    private readonly ICountriesService _countriesService;
    

    public PersonsService()
    {
        _people = new List<Person>();
        _countriesService = new CountriesService();
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();

        personResponse.CountryName = _countriesService.GetCountryByCountryId(personResponse.CountryId)?.CountryName;
        
        return personResponse;
    }
    
    public PersonResponse? AddPerson(PersonAddRequest? personAddRequest)
    {
        ArgumentNullException.ThrowIfNull(personAddRequest);
        
        ValidationHelper.ModelValidation(personAddRequest);
        
        var person = personAddRequest.ToPerson();
        
        person.PersonId =  Guid.NewGuid();
        
        _people.Add(person);
        
        return ConvertPersonToPersonResponse(person);
    }
    
    public List<PersonResponse> GetAllPersons()
    {
        return _people.Select(c => c.ToPersonResponse()).ToList();
    }

    public PersonResponse? GetPersonByPersonId(Guid? personId)
    {
        if (personId is null)
            return null;
        
        Person? person = _people.FirstOrDefault(c=> c.PersonId == personId);
        return person?.ToPersonResponse();
    }
    
    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetAllPersons();

        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchString))
            return matchingPersons;
        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPersons = allPersons.Where(c =>
                    (string.IsNullOrWhiteSpace(c.PersonName) || c.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); 
                break;
            case nameof(Person.Email):
                matchingPersons = allPersons.Where(c =>
                    (string.IsNullOrWhiteSpace(c.Email) || c.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); 
                break;
            case nameof(Person.DateOfBirth):
                matchingPersons = allPersons.Where(c =>
                    (c.DateOfBirth is not null || c.DateOfBirth!.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); 
                break;
            case nameof(Person.Gender):
                matchingPersons = allPersons.Where(c =>
                    (c.Gender is not null || c.Gender.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); 
                break;
            case nameof(Person.CountryId):
                matchingPersons = allPersons.Where(c =>
                    (string.IsNullOrWhiteSpace(c.CountryName) || c.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); 
                break;
            case nameof(Person.Address):
                matchingPersons = allPersons.Where(c =>
                    (string.IsNullOrWhiteSpace(c.Address) || c.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); 
                break;
            default: matchingPersons = allPersons;
                break;
        }
        
        return matchingPersons;
    }
    
    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrderOptions)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return allPersons;

        List<PersonResponse> sortedPersons = (sortBy, sortOrderOptions)
            switch
            {
                (nameof(PersonResponse.PersonName) , SortOrderOptions.Asc) 
                    => allPersons.OrderBy(c=> c.PersonName , StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName) , SortOrderOptions.Desc) 
                    => allPersons.OrderByDescending(c=> c.PersonName , StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.Email) , SortOrderOptions.Asc) 
                    => allPersons.OrderBy(c=> c.PersonName , StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email) , SortOrderOptions.Desc) 
                    => allPersons.OrderByDescending(c=> c.PersonName , StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.DateOfBirth) , SortOrderOptions.Asc) 
                    => allPersons.OrderBy(c=> c.DateOfBirth ).ToList(),
                (nameof(PersonResponse.DateOfBirth) , SortOrderOptions.Desc) 
                    => allPersons.OrderByDescending(c=> c.DateOfBirth).ToList(),
                
                (nameof(PersonResponse.Age) , SortOrderOptions.Asc) 
                    => allPersons.OrderBy(c=> c.Age ).ToList(),
                (nameof(PersonResponse.Age) , SortOrderOptions.Desc) 
                    => allPersons.OrderByDescending(c=> c.Age).ToList(),
                
                (nameof(PersonResponse.Gender) , SortOrderOptions.Asc) 
                    => allPersons.OrderBy(c=> c.Gender).ToList(),
                (nameof(PersonResponse.Gender) , SortOrderOptions.Desc) 
                    => allPersons.OrderByDescending(c=> c.Gender).ToList(),
                
                (nameof(PersonResponse.CountryName) , SortOrderOptions.Asc) 
                    => allPersons.OrderBy(c=> c.CountryName , StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.CountryName) , SortOrderOptions.Desc) 
                    => allPersons.OrderByDescending(c=> c.CountryName , StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.Address) , SortOrderOptions.Asc) 
                    => allPersons.OrderBy(c=> c.Address , StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address) , SortOrderOptions.Desc) 
                    => allPersons.OrderByDescending(c=> c.Address , StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.ReceiveNewsLetters) , SortOrderOptions.Asc) 
                    => allPersons.OrderBy(c=> c.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters) , SortOrderOptions.Desc) 
                    => allPersons.OrderByDescending(c=> c.ReceiveNewsLetters).ToList(),
                
                _ => allPersons
            };
        
        return sortedPersons;
    }

    public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest is null)
            throw new ArgumentNullException(nameof(personUpdateRequest));
        
        ValidationHelper.ModelValidation(personUpdateRequest);

        Person? matchingPerson = _people.FirstOrDefault(c => c.PersonId == personUpdateRequest.PersonId);
        if (matchingPerson is null)
        {
            throw new ArgumentException("Given person id doesn't exist");
        }

        matchingPerson.PersonName = personUpdateRequest.PersonName;

        return matchingPerson.ToPersonResponse();
    }
    
    public bool DeletePerson(Guid? personId)
    {
        if (personId is null)
        {
            throw new ArgumentNullException(nameof(personId));
        }
        
        Person? person = _people.FirstOrDefault(c=> c.PersonId == personId);
        if (person is null)
            return false;
        
        _people.Remove(person);
        return true;
    }
}
using System;
using Entities;
using Extensions;
using ServiceContracts.DTO.Request;

namespace ServiceContracts.DTO.Response;

public class PersonResponse
{
    public Guid PersonId { get; set; }
    
    public string? PersonName { get; set; }
    
    public string? Email { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    public double? Age { get; set; }
    
    public GenderOptions? Gender { get; set; }
    
    public Guid? CountryId { get; set; }
    
    public string? CountryName { get; set; }
    
    public string? Address { get; set; }
    
    public bool ReceiveNewsLetters { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj.GetType() != typeof(PersonResponse)) return false;
        
        PersonResponse? personToCompare = obj as PersonResponse;

        return personToCompare is not null && this.PersonId == personToCompare.PersonId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return new PersonUpdateRequest()
        {
            PersonId = this.PersonId,
            PersonName = this.PersonName,
            Email = this.Email,
            DateOfBirth = this.DateOfBirth,
            Gender = this.Gender,
            CountryId = this.CountryId,
        };
    }
}

public static class PersonExtensions
{
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new PersonResponse
        {
            PersonId = person.PersonId,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender =  person.Gender,
            CountryId = person.CountryId,
            Address = person.Address,
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            Age = person.DateOfBirth is not null ? Convert.ToDouble(DateTime.Now.Year - person.DateOfBirth.Value.Year) : null
        };
    }
}
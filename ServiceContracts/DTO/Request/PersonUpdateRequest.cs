using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using Extensions;

namespace ServiceContracts.DTO.Request;

public class PersonUpdateRequest
{
    [Required(ErrorMessage = "Person id can not be blank")]
    public Guid PersonId { get; set; }
    
    [Required(ErrorMessage = "PersonName can not be blank")]
    public string? PersonName { get; set; }
    
    [Required(ErrorMessage = "Email can not be blank")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string? Email { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    public GenderOptions? Gender { get; set; }
    
    public Guid? CountryId { get; set; }

    public Person ToPerson()
    {
        return new Person()
        {
            PersonId = this.PersonId,
            PersonName = this.PersonName,
            Email = this.Email,
            DateOfBirth = this.DateOfBirth,
            Gender = this.Gender,
            CountryId = this.CountryId
        };
    }
}
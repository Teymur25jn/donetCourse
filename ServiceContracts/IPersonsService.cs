using Extensions.ENUMs;
using ServiceContracts.DTO.Request;
using ServiceContracts.DTO.Response;

namespace ServiceContracts;

public interface IPersonsService
{
    public PersonResponse? AddPerson(PersonAddRequest? personAddRequest);
    
    public List<PersonResponse> GetAllPersons();
    
    public PersonResponse? GetPersonByPersonId(Guid? personId);

    public List<PersonResponse> GetFilteredPersons(string searchBy , string? searchString);

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons , string sortBy , SortOrderOptions sortOrderOptions);
    
    public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    public bool DeletePerson(Guid? personId);
}
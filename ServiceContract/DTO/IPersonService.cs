using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ServiceContract.DTO
{
    public interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest personAddRequest);
        List<PersonResponse> GetAllPersons();
    }
}

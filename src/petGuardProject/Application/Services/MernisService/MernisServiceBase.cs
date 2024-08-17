using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.MernisService;

public abstract class MernisServiceBase
{
    public abstract Task CheckIfRealPerson(long nationalityNumber,string firstName,string lastName, int yearOfBirth);
}

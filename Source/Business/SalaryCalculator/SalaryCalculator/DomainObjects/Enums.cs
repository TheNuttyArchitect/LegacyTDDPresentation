using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator.DomainObjects
{
    public enum DeductionType
    {
        MedicalInsurance = 1,
        DentalInsurance = 2,
        VisionInsurance = 3,
        ShortTermDisability = 4,
        LongTermDisability = 5,
        FourOOneK = 6
    }

    public enum TaxType
    {
        Federal = 1,
        State = 2,
        Local = 3,
        FICA = 4
    }
}

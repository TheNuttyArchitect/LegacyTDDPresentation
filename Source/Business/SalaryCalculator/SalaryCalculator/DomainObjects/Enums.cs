using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator.DomainObjects
{
    public enum DeductionType
    {
        MedicalInsurance,
        DentalInsurance,
        VisionInsurance,
        ShortTermDisability,
        LongTermDisability,
        FourOOneK
    }

    public enum TaxType
    {
        Federal,
        State,
        Local,
        FICA
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator.DomainObjects
{
    public abstract class Debit<TSubType>
    {
        public decimal Amount { get; set; }
        public TSubType DebitType { get; set; }
    }
}

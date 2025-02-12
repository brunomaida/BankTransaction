using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib.Rules
{  
   /// <summary>
   /// Allows comparison between several strings, numbers, dates and everything that allows it
   /// </summary>
    [Serializable]
    public enum ComparisonType
    {
        None = -1,
        EqualTo = 0,
        NotEqualTo = 1,
        GreaterThan = 2,
        GreaterOrEqualTo = 3,
        LowerThan = 4,
        LowerOrEqualTo = 5
    }
}

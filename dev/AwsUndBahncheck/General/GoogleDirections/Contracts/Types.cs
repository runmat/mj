using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleDirections.Contracts
{
    public delegate bool PredicateAddressCaching(string startAddress, string endAddress, out double outParam);
    public delegate void ActionAddressCaching(string startAddress, string endAddress, double outParam);
}

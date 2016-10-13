using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2
{
    public interface IHaveCost : ITalent
    {
        int Cost { get; set; }
    }
}
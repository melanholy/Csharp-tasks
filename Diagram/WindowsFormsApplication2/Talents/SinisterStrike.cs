using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2
{
    public class SinisterStrike : IRogueTalent, IHaveCost, IPower, ICastable
    {
        public int Cost
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Effect Effect
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Cast(ICharacter[] targets)
        {
            throw new NotImplementedException();
        }
    }
}
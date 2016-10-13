using System.Collections.Generic;

namespace WindowsFormsApplication2
{
    public interface ICharacter
    {
        int Health { get; set; }
        double WalkSpeed { get; set; }
        double SpellResist { get; set; }
        double ResourceRegen { get; set; }

        void Dispel(Effect effect);
        void GetEffect(Effect effect);
        
    }
}
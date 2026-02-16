using Terraria.ModLoader;

namespace CalRemix.Content.DamageClasses
{
    public class StormbowDamageClass : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == DamageClass.Generic)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }

        public override bool UseStandardCritCalcs => true;
    }
}
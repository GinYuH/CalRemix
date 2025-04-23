using Terraria.ModLoader;

namespace CalRemix.Content.DamageClasses
{
    public class FarmingDamageClass : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic)
                return new StatInheritanceData(critChanceInheritance: 0.5f);
            return StatInheritanceData.None;
        }
        public override bool UseStandardCritCalcs => true;
    }
}
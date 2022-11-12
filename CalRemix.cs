using CalamityMod;
using CalamityMod.UI.CalamitasEnchants;
using CalRemix.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix
{
	public class CalRemix : Mod
	{
        public override void PostSetupContent()
        {
            ModLoader.GetMod("CalamityMod").Call("RegisterModCooldowns", this);
            ModLoader.GetMod("CalamityMod").Call("DeclareMiniboss", ModContent.NPCType<LifeSlime>());
            EnchantmentManager.EnchantmentList.Add(new Enchantment("Fallacious", "Greatly increases critical strike damage but critical strike chance is reduced. Critical hits also hurt you.", 156, "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneFallacious", null, delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().amongusEnchant = true;
            }, (Item item) => item.IsEnchantable() && item.damage > 0 && !item.CountsAsClass<SummonDamageClass>() && !item.IsWhip()));
            EnchantmentManager.EnchantmentList.Add(new Enchantment("Defiant", "Dealing damage increases defense and damage but defense damage taken is increased.", 157, "CalRemix/ExtraTextures/Enchantments/EnchantmentRuneDefiant", null, delegate (Player player)
            {
                player.GetModPlayer<CalRemixPlayer>().earthEnchant = true;
            }, (Item item) => item.IsEnchantable() && item.damage > 0 && !item.CountsAsClass<SummonDamageClass>() && !item.IsWhip()));
        }
    }
}
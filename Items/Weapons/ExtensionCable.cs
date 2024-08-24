using CalamityMod;
using CalamityMod.Items;
using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons
{
    public class ExtensionCable : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Main.rand.NextBool() ? "Extension Cable" : "Extension Cord"); // Legally speaking, Cable is the canon name
            Tooltip.SetDefault("Tagged enemies are struck by lightning when hit by minions");
        }

        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<ExtensionCordWhip>(), 180, 2, 5);
            Item.rare = RarityHelper.Ionogen;
            Item.channel = true;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
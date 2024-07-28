using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.NPCs;
using Terraria.Audio;

namespace CalRemix.Items.Weapons
{
    public class PaletteUncleanser : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Palette Uncleanser");
            Tooltip.SetDefault("Causes hit enemies to become vulnerable to all debuff elements");
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = RarityHelper.Origen;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.UseSound = BetterSoundID.ItemBoingReflectProjectile;
            Item.DamageType = DamageClass.Generic;
            Item.damage = 16;
            Item.knockBack = 2f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.TryGetGlobalNPC(out CalamityGlobalNPC cgn))
            {
                cgn.VulnerableToCold = true;
                cgn.VulnerableToElectricity = true;
                cgn.VulnerableToHeat = true;
                cgn.VulnerableToSickness = true;
                cgn.VulnerableToHeat = true;
                SoundEngine.PlaySound(BetterSoundID.ItemMagicIceBlock, target.Center);
            }
        }
    }
}

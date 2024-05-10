using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Rarities;

namespace CalRemix.Items
{
    public class RipperShark : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 105;
            Item.knockBack = 10f;
            Item.useTime = 12;
            Item.useAnimation = 17;
            Item.pick = 59;

            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 60;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare =  ModContent.RarityType<PureGreen>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 30;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 480);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust d = CalamityUtils.MeleeDustHelper(player, DustID.Water, 0.56f, 40, 65, -0.13f, 0.13f);
            if (d != null)
            {
                d.customData = 0.02f;
            }
        }
    }
}

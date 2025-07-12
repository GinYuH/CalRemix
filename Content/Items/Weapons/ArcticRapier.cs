using CalRemix.Content.Projectiles.Weapons;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Ranged;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Weapons
{
    public class ArcticRapier : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 132;
            Item.knockBack = 7f;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();

            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.autoReuse = true;
            Item.UseSound = BetterSoundID.ItemIceBreak;

            Item.width = 24;
            Item.height = 24;

            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;

            Item.shoot = ModContent.ProjectileType<IcicleArrowProj>();
            Item.shootSpeed = 18f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -2; i < 3; i++)
            {
                Vector2 pos = position + new Vector2(i * 120, -800);
                int p = Projectile.NewProjectile(source, pos, pos.DirectionTo(Main.MouseWorld) * 5, type, damage, knockback, player.whoAmI);
                Main.projectile[p].DamageType = DamageClass.Melee;
                Main.projectile[p].localAI[0] = 100;
                Main.projectile[p].ModProjectile<IcicleArrowProj>().falling = true;
                Main.projectile[p].alpha = 255;
            }
            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.Distance(player.Center) < 640)
                    {
                        n.AddBuff(ModContent.BuffType<GlacialState>(), 300);
                    }
                }
            }
            return true;
        }
    }
}

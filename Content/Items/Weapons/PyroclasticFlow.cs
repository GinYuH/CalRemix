using System;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class PyroclasticFlow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates enormous explosions on enemy hits");
        }
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.damage = 447;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 35;
            Item.useTime = 35;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7.25f;
            Item.scale = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LargeWeaponFireSound, player.Center);
            GeneralParticleHandler.SpawnParticle(new DetailedExplosion(target.Center, Vector2.Zero, Color.Orange, Vector2.One, Main.rand.NextFloat(-3.14f, 3.14f), 0.3f, 2, 10));
            player.Calamity().GeneralScreenShakePower = 6;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.Distance(target.Center) < 400 && target.whoAmI != n.whoAmI)
                {
                    int p = Projectile.NewProjectile(player.GetSource_FromThis(), n.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), damageDone, Item.knockBack, player.whoAmI, n.whoAmI);
                    Main.projectile[p].DamageType = DamageClass.Melee;
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LargeWeaponFireSound, player.Center);
            GeneralParticleHandler.SpawnParticle(new DetailedExplosion(target.Center, Vector2.Zero, Color.Orange, Vector2.One, Main.rand.NextFloat(-3.14f, 3.14f), 0.3f, 2, 10));
            player.Calamity().GeneralScreenShakePower = 6;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int fireDust = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, DustID.Torch, (float)(player.direction * 2), 0f, 150, default, 1.5f);
                Main.dust[fireDust].velocity *= 0.2f;
            }
        }
    }
}

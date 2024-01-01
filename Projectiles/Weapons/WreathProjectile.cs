using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class WreathProjectile : ModProjectile
    {
        public override string Texture => "CalRemix/Items/Weapons/WreathofBelial";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wreath");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.timeLeft = 600;
            Projectile.penetrate = 5;
        }

        public override void AI()
        {
            if (Projectile.Calamity().stealthStrike)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n == null || !n.active)
                        continue;
                    Vector2 distToWreath = n.DirectionTo(Projectile.Center);
                    if (n.Distance(Projectile.Center) < 400)
                    {
                        n.velocity += distToWreath * 1.0002f;
                    }
                }
            }
            Projectile.scale *= 1.002f;
            Projectile.rotation += 0.04f * Math.Sign(Projectile.velocity.X);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(6, 9); i++)
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f)) * Main.rand.NextFloat(4f, 8f), ModContent.ProjectileType<FrostShardFriendly>(), Projectile.damage / 4, 0, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D wreath = Projectile.Calamity().stealthStrike ? ModContent.Request<Texture2D>("CalRemix/ExtraTextures/DarkWreath").Value : TextureAssets.Projectile[Type].Value;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, texture: wreath);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
        }
    }
}
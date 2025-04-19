using CalamityMod;
using CalRemix.Content.NPCs.Eclipse;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PizzaWheelHostile : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/PizzaWheel";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pizza");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 300;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation += 0.04f * Math.Sign(Projectile.velocity.X);
            NPC freddy = Main.npc[(int)Projectile.ai[0]];
            if (freddy != null && freddy.active && freddy.type == ModContent.NPCType<EvilAnimatronic>())
            {
                if (Projectile.timeLeft <= 220 && Projectile.ai[1] == 0)
                {
                    Projectile.velocity = Projectile.DirectionTo(freddy.Center) * 22;
                    if (Projectile.getRect().Intersects(freddy.getRect()))
                    {
                        Projectile.active = false;
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 22; k++)
            {
                Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Blood, Scale: 2f + Main.rand.NextFloat())];
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D wreath = Projectile.Calamity().stealthStrike ? ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/DarkWreath").Value : TextureAssets.Projectile[Type].Value;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, texture: wreath);
            return false;
        }
    }
}
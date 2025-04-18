using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class Scrap : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Weapons/Scrap1";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Scrap");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 240;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
        }
        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.05f;
            if (Projectile.velocity.Y > 12)
            {
                Projectile.velocity.Y = 12;
            }
            else
            {
                Projectile.velocity.Y += 0.12f;
            }
            if (Projectile.ai[1] == 0)
            {
                Projectile.timeLeft *= 2;
                Projectile.ai[1] = 1;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.Calamity().stealthStrike)
                target.AddBuff(BuffID.Electrified, 120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.Calamity().stealthStrike)
            target.AddBuff(BuffID.Electrified, 120);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2));
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Weapons/Scrap" + Projectile.ai[0]).Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            if (Projectile.Calamity().stealthStrike)
                CalamityUtils.DrawProjectileWithBackglow(Projectile, Color.Yellow * 0.4f, lightColor, 4, texture);
            else
                Main.EntitySpriteDraw(texture, centered, null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
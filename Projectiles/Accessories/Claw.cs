using CalamityMod.CalPlayer;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Particles;
using CalRemix.Buffs;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace CalRemix.Projectiles.Accessories
{
    public class Claw : ModProjectile
    {
        public ref float target => ref Projectile.ai[0];
        public ref float time => ref Projectile.ai[1];
        public override string Texture => "CalRemix/Projectiles/Accessories/ClawTop";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Claw");
        }

        public override void SetDefaults()
        {
            Projectile.width = 260;
            Projectile.height = 80;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            NPC victim = Main.npc[(int)target];
            if (victim == null || !victim.active || victim.dontTakeDamage)
            {
                victim = CalamityUtils.MinionHoming(Projectile.Center, 1000, Main.player[Projectile.owner]);
                if (victim != null && victim.active && !victim.dontTakeDamage)
                    target = victim.whoAmI;
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                if (Projectile.Center.Distance(victim.Center) > 90 && victim.GetGlobalNPC<CalRemixGlobalNPC>().clawed <= 0)
                {
                    /* Vector2 dist = victim.Center - Projectile.Center;
                     dist.Normalize();
                     Projectile.velocity = dist * 10;*/
                    float strength = 0.05f;
                    Projectile.position.X = MathHelper.Lerp(Projectile.position.X, victim.Center.X + 120 * Projectile.spriteDirection - Projectile.width / 2, strength);
                    Projectile.position.Y = MathHelper.Lerp(Projectile.position.Y, victim.Center.Y - Projectile.height / 2, strength);
                    Projectile.ai[2] = MathHelper.Clamp(Projectile.ai[2] + MathHelper.ToRadians(4), 0.1f, 0.7f);
                }
                else
                {
                    Projectile.ai[2] = MathHelper.Clamp(Projectile.ai[2] - MathHelper.ToRadians(5), 0.1f, 0.7f);
                    Projectile.Center = victim.Center + Vector2.UnitX * 120 * Projectile.spriteDirection;
                }
            }
            Projectile.direction = Math.Sign(Projectile.velocity.X);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D ClawTop = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D ClawBottom = ModContent.Request<Texture2D>("CalRemix/Projectiles/Accessories/ClawBottom").Value;
            SpriteEffects sfx = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Rectangle ClawTopRect = new Rectangle(0, 0, ClawTop.Width, ClawBottom.Height);
            Rectangle ClawBottomRect = new Rectangle(0, 0, ClawTop.Width, ClawBottom.Height);

            float bottomRotation = 0;
            float topRotation = 0;

            NPC victim = Main.npc[(int)target];
            if (victim != null && victim.active && !victim.dontTakeDamage)
            {
                float enemyDist = Projectile.Center.Distance(victim.Center);
                float enemyDistCapped = MathHelper.Clamp(enemyDist, 0, 30);
                topRotation = MathHelper.Clamp(30 / enemyDistCapped, -MathHelper.PiOver2, 0);
                bottomRotation = MathHelper.Clamp(30 / enemyDistCapped, 0, MathHelper.PiOver2);
            }

            Main.EntitySpriteDraw(ClawBottom, Projectile.Center - Main.screenPosition, null, lightColor with { A = 255 }, -Projectile.ai[2] * Projectile.spriteDirection, Utils.Size(ClawBottomRect) / 2f + ClawBottomRect.Width / 2 * Vector2.UnitX * Projectile.spriteDirection, Projectile.scale, sfx, 0);
            Main.EntitySpriteDraw(ClawTop, Projectile.Center - Main.screenPosition, null, lightColor with { A = 255 }, Projectile.ai[2] * Projectile.spriteDirection, Utils.Size(ClawTopRect) / 2f + ClawTopRect.Width / 2 * Vector2.UnitX * Projectile.spriteDirection, Projectile.scale, sfx, 0);
        }

        public override void OnHitNPC(NPC targete, NPC.HitInfo hit, int damageDone)
        {
            if (targete.active && targete.life > 0 && targete.whoAmI == target && !targete.dontTakeDamage)
            {
                if (targete.GetGlobalNPC<CalRemixGlobalNPC>().clawed <= 0)
                {
                    SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ScissorGuillotineSnapSound);
                }
                targete.GetGlobalNPC<CalRemixGlobalNPC>().clawed = 180;
                if (Main.player[Projectile.owner].active && Main.player[Projectile.owner].GetModPlayer<CalRemixPlayer>().clawPosition != Vector2.Zero)
                {
                    targete.GetGlobalNPC<CalRemixGlobalNPC>().clawPosition = Main.player[Projectile.owner].GetModPlayer<CalRemixPlayer>().clawPosition;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            NPC latched = Main.npc[(int)target];
            if (latched.active && latched != null && latched.GetGlobalNPC<CalRemixGlobalNPC>().clawed > 0)
            {
                latched.GetGlobalNPC<CalRemixGlobalNPC>().clawed = 0;
            }
        }
    }
}

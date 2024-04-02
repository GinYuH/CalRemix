using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
            Projectile.width = 120;
            Projectile.height = 80;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].ownedProjectileCounts[Type] < 2)
            {
                Projectile.Kill();
            }
            NPC victim = Main.npc[(int)target];
            if (victim == null || !victim.active || victim.dontTakeDamage || !victim.chaseable || victim.townNPC || victim.CountsAsACritter)
            {
                victim = CalamityUtils.MinionHoming(Projectile.Center, 1000, Main.player[Projectile.owner]);
                if (victim != null && victim.active && !victim.dontTakeDamage)
                    target = victim.whoAmI;
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                float clawDist = 40;
                Vector2 enemyWidth = Vector2.UnitX * victim.width / 2.5f * Projectile.spriteDirection;
                if (Projectile.Center.Distance(victim.Center) > 60 && victim.GetGlobalNPC<CalRemixGlobalNPC>().clawed <= 0)
                {
                    /* Vector2 dist = victim.Center - Projectile.Center;
                     dist.Normalize();
                     Projectile.velocity = dist * 10;*/
                    float strength = 0.05f;
                    Projectile.position.X = MathHelper.Lerp(Projectile.position.X, victim.Center.X + clawDist * Projectile.spriteDirection - Projectile.width / 2 + enemyWidth.X, strength);
                    Projectile.position.Y = MathHelper.Lerp(Projectile.position.Y, victim.Center.Y - Projectile.height / 2, strength);
                    Projectile.ai[2] = MathHelper.Clamp(Projectile.ai[2] + MathHelper.ToRadians(4), -0.05f, 0.7f);
                }
                else
                {
                    Projectile.ai[2] = MathHelper.Clamp(Projectile.ai[2] - MathHelper.ToRadians(5), -0.05f, 0.7f);
                    Projectile.Center = victim.Center + Vector2.UnitX * clawDist * Projectile.spriteDirection + enemyWidth;
                }
            }
            Projectile.direction = Math.Sign(Projectile.velocity.X);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D ClawTop = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D ClawBottom = ModContent.Request<Texture2D>("CalRemix/Projectiles/Accessories/ClawBottom").Value;
            SpriteEffects sfx = Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Rectangle ClawTopRect = new Rectangle(0, 0, ClawTop.Width, ClawBottom.Height);
            Rectangle ClawBottomRect = new Rectangle(0, 0, ClawTop.Width, ClawBottom.Height);

            Main.EntitySpriteDraw(ClawTop, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.ai[2] * Projectile.spriteDirection, new Vector2(49, 31), Projectile.scale, sfx, 0);
            Main.EntitySpriteDraw(ClawBottom, Projectile.Center - Main.screenPosition, null, lightColor, -Projectile.ai[2] * Projectile.spriteDirection / 2, Utils.Size(ClawBottomRect) / 2f, Projectile.scale, sfx, 0);
            return false;
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

        public override void OnKill(int timeLeft)
        {
            NPC latched = Main.npc[(int)target];
            if (latched.active && latched != null && latched.GetGlobalNPC<CalRemixGlobalNPC>().clawed > 0)
            {
                latched.GetGlobalNPC<CalRemixGlobalNPC>().clawed = 0;
            }
            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 4; i++)
                {
                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, ModLoader.GetMod("CalRemix").Find<ModGore>("Claw" + (i + 1)).Type, 1f);
                }
            }
            SoundEngine.PlaySound(SoundID.DD2_SkeletonDeath, Projectile.Center);
        }
    }
}

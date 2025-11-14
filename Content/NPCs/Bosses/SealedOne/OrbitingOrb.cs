using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.SealedOne
{
    public class OrbitingOrb : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public ref float SpinDuration => ref Projectile.ai[1];
        public ref float Mode => ref Projectile.ai[2];
        public override string Texture => "CalRemix/Assets/Gores/Derellect1";

        public Vector2 IdealLocation = Vector2.Zero;
        public bool FUCKYOU = false;
        public float StartingSpeed = 1;
        Vector2? directionToPlayer = null;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Orbiting Orb");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CultistBossLightningOrb);
            Projectile.alpha = 0;
            Projectile.frame = 1;
            Projectile.aiStyle = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SpinDuration = Main.rand.Next(0, 366);
            StartingSpeed = Main.rand.NextFloat(0.8f, 1.1f);
        }

        public override void AI()
        {
            int distanceFromPlayer = 450;
            float slowDownEasing = 100;
            if (Timer > 80)
            {
                if (!FUCKYOU)
                    slowDownEasing = 1 - CalamityUtils.CircOutEasing(Mode / 150f, 1);
                if (slowDownEasing <= 0)
                    FUCKYOU = true;
                Mode++;
            }
            if (FUCKYOU)
                slowDownEasing = 0;
            else if (slowDownEasing >= StartingSpeed)
                slowDownEasing = StartingSpeed;
            double radians = SpinDuration / 10;
            //Vector2 IdealPosition = Main.LocalPlayer.Center + Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, Utils.GetLerpValue(50, 300, Timer, true))) * distanceFromPlayer;
            Vector2 IdealPosition = Main.LocalPlayer.Center + Vector2.UnitY.RotatedBy(radians) * distanceFromPlayer;

            if (!FUCKYOU)
            {
                Projectile.position = IdealPosition;
                Projectile.position.X -= Projectile.width / 2;
                Projectile.position.Y -= Projectile.height / 2;
            }
            else
            {
                if (directionToPlayer == null)
                {
                    directionToPlayer = Projectile.Center.DirectionTo(Main.LocalPlayer.Center);

                    SoundEngine.PlaySound(SoundID.Zombie5 with { MaxInstances = 0, Volume = 2f, Pitch = Main.rand.NextFloat(-3.0f, 3.0f) });
                    for (int i = 0; i < 15; i++)
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1), 0, default, 1f);
                    
                    Timer = 0;
                }
                float rampup = (float)(Math.Pow(Timer, 2) / 150);
                if (rampup > 2.5f)
                    rampup = 2.5f;
                Main.NewText(rampup);
                Projectile.velocity += (Vector2)directionToPlayer * rampup;
            }

                SpinDuration += slowDownEasing;
            if (slowDownEasing <= 0)
                slowDownEasing = 0;

            Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Assets/Gores/Derellect1").Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
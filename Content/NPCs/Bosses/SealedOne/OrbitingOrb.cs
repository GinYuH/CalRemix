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

        public enum Modes
        {
            RotateAroundPlayer = 0,
            GoInwards = 1
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orbiting Orb");
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
            Timer = Main.rand.Next(0, 365);
            //SpinDuration = Main.rand.NextFloat(0.5f, 0.75f);
            SpinDuration = 0.01f;
        }

        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];

            if (Mode == (float)Modes.RotateAroundPlayer)
            {
                int distanceFromPlayer = 450;
                float IdealPositionX = (float)(player.MountedCenter.X - (Math.Cos((Timer * 0.06f) - SpinDuration) * distanceFromPlayer));
                float IdealPositionY = (float)(player.MountedCenter.Y - (Math.Sin((Timer * 0.06f) - SpinDuration) * distanceFromPlayer));
                Projectile.Center = new Vector2(IdealPositionX, IdealPositionY);

                // the number 2.45 was pulled out of my ass after i trial-and-errored a good spot for this thing to stop
                // i did this because i do not want to work with this projectile anymore 
                // everything about this is unelegant and unintelligently done
                // i believe this is the... "fabsol style"
                if (SpinDuration < 2.45f)
                {
                    Timer++;
                    SpinDuration *= 1.025f;
                }
                else
                {
                    Mode = (float)Modes.GoInwards;
                    Timer = 0;
                }
            }
            else if (Mode == (float)Modes.GoInwards)
            {
                // telegraph stuff, save player location
                if (Timer == 0)
                {
                    SoundEngine.PlaySound(SoundID.Zombie5 with { MaxInstances = 0, Volume = 2f, Pitch = Main.rand.NextFloat(-3.0f, 3.0f) });
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1), 0, default, 1f);
                    }

                    IdealLocation = Projectile.DirectionTo(player.Center);
                }

                // go to place
                Projectile.velocity += IdealLocation * 0.8f;

                Timer++;
            }
            else
            {
                // just in case
                Projectile.active = false;
            }
            
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
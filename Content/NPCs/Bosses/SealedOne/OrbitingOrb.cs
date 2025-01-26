using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.SealedOne
{
    public class OrbitingOrb : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public ref float SpinDuration => ref Projectile.ai[1];
        public override string Texture => "CalRemix/Assets/Gores/Derellect1";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orbiting Orb");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CultistBossLightningOrb);
            Projectile.frame = 1;
            Projectile.aiStyle = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Timer = Main.rand.Next(0, 365);
            //SpinDuration = Main.rand.NextFloat(0.5f, 0.75f);
        }

        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];
            Main.NewText(SpinDuration);
            Main.NewText(SpinDuration / 400f);

            int distanceFromPlayer = 450;
            float IdealPositionX = (float)(player.MountedCenter.X - (Math.Cos((Timer * 0.05f) - (SpinDuration)) * distanceFromPlayer));
            float IdealPositionY = (float)(player.MountedCenter.Y - (Math.Sin((Timer * 0.05f) - (SpinDuration)) * distanceFromPlayer));
            Projectile.Center = new Vector2(IdealPositionX, IdealPositionY);

            Timer++;
            SpinDuration += 0.25f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Assets/Gores/OxygenShrap1").Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
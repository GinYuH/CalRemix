using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
    public class HeavenPiercerLaser : ModProjectile
    {
        public ref float Time => ref Projectile.ai[0];
        public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("Helix Laser");
		}
		public override void SetDefaults() 
		{
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.aiStyle = -1;
            Projectile.width = 4;
			Projectile.height = 4;
            Projectile.light = 0.75f;
            Projectile.timeLeft = 120;
            Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255);
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Time += 0.25f;
            float wave = (float)Math.Sin((Time) + Math.PI / 2) * 10f;
            Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(Math.PI / 2);
            if (Time % 0.5 == 0)
            {
                Dust.NewDust(Projectile.Center + direction * wave, 1, 1, DustID.ShadowbeamStaff);
                Dust.NewDust(Projectile.Center + -direction * wave, 1, 1, DustID.ShadowbeamStaff);
            }
            int mineRadius = 1;
            int minTileX = (int)(Projectile.Center.X / 16f - (float)mineRadius);
            int maxTileX = (int)(Projectile.Center.X / 16f + (float)mineRadius);
            int minTileY = (int)(Projectile.Center.Y / 16f - (float)mineRadius);
            int maxTileY = (int)(Projectile.Center.Y / 16f + (float)mineRadius);
            if (minTileX < 0)
                minTileX = 0;
            if (maxTileX > Main.maxTilesX)
                maxTileX = Main.maxTilesX;
            if (minTileY < 0)
                minTileY = 0;
            if (maxTileY > Main.maxTilesY)
                maxTileY = Main.maxTilesY;

            for (int i = minTileX; i <= maxTileX; i++)
            {
                for (int j = minTileY; j <= maxTileY; j++)
                {
                    float diffX = Math.Abs((float)i - Projectile.Center.X / 16f);
                    float diffY = Math.Abs((float)j - Projectile.Center.Y / 16f);
                    double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distanceToTile < (double)mineRadius)
                    {
                        bool canKillTile = true;
                        if (Main.tile[i, j] != null && Main.tile[i, j].HasTile)
                        {
                            canKillTile = true;
                            if (Main.tile[i, j].TileType == TileID.Containers || Main.tile[i, j].TileType == TileID.Containers2)
                                canKillTile = false;
                            if (canKillTile)
                            {
                                WorldGen.KillTile(i, j, false, false, false);
                                if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                                {
                                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

    }

}

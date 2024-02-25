using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
	public class LaserSlash : ModProjectile
	{
        public override string Texture => "CalamityMod/Projectiles/Melee/MurasamaSlash";
        private int frameX;
		public override void SetStaticDefaults() 
        {
            Main.projFrames[Type] = 7;
        }
		public override void SetDefaults()
        {
            Projectile.width = 216;
            Projectile.height = 216;
            Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.scale = 0.75f;
            if (Main.rand.NextBool(2))
                Projectile.frame = 6;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ELRFireSound, Projectile.Center);
            if (Projectile.ai[0] == 1)
            {
                Projectile.timeLeft = 20;
            }
            else
            {
                Projectile.timeLeft = 120;
            }
        }
        public override void AI()
        {
            foreach (Player player in Main.player)
            {
                if (player.getRect().Intersects(Projectile.getRect()) && player.immuneTime <= 0)
                {
                    if (CalamityWorld.revenge)
                        player.Hurt(PlayerDeathReason.ByProjectile(Projectile.owner, Projectile.whoAmI), (int)(player.statLifeMax2 / 10f), (player.Center.X > Projectile.Center.X) ? 1 : -1, dodgeable: false, armorPenetration: 10000);
                    else if (Main.expertMode)
                        player.Hurt(PlayerDeathReason.ByProjectile(Projectile.owner, Projectile.whoAmI), (int)(player.statLifeMax2 / 20f), (player.Center.X > Projectile.Center.X) ? 1 : -1, dodgeable: false, armorPenetration: 10000);
                    else
                        player.Hurt(PlayerDeathReason.ByProjectile(Projectile.owner, Projectile.whoAmI), (int)(player.statLifeMax2 / 40f), (player.Center.X > Projectile.Center.X) ? 1 : -1, dodgeable: false, armorPenetration: 10000);
                }
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 2)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 5)
                Projectile.frame = 0;
        }
        public override bool PreDraw(ref Color lightColor) 
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle rect = texture.Frame(1, 14, frameX, Projectile.frame);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Color.White, Projectile.rotation, rect.Size() / 2f, Projectile.scale, (Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
		}
    }
}
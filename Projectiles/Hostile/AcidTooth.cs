using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
	public class AcidTooth : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Acid Shot");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 10;
			Projectile.height = 10;
            Projectile.timeLeft = 600;
			Projectile.hostile = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            int index = Player.FindClosest(Projectile.Center, 1, 1);
            if (Main.player[index] == null || !index.WithinBounds(Main.maxPlayers))
                return;
            else if (Main.player[index].dead || !Main.player[index].active)
                return;
            Player target = Main.player[index];
            Projectile.velocity = (Projectile.velocity * 119f + Projectile.DirectionTo(target.Center) * 8.5f / 1.5f) / 120f;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityDusts.SulfurousSeaAcid, Scale: 1f + Main.rand.NextFloat());
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.spriteBatch.Draw(texture, centered, null, new Color(255, 255, 255), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
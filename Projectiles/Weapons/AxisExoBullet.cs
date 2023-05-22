using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class AxisExoBullet : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Exo Bullet");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.penetrate = 10;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.extraUpdates = 10;
            AIType = ProjectileID.Bullet;
		}
		public override void AI()
		{
            NPC target = Projectile.FindTargetWithinRange(240f);
            if (Projectile.ai[0] == 1)
            {
				if (target != null)
				{
                    Projectile.alpha = 0;
                    Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.One) * 12f; 
                    if (!Main.dedServ)
                    {
                        for (int l = 0; l < 12; l++)
                        {
                            Vector2 spinningpoint = Vector2.UnitX * -Projectile.width / 2f;
                            spinningpoint += -Vector2.UnitY.RotatedBy(l * (float)Math.PI / 6f) * new Vector2(8f, 16f);
                            spinningpoint = spinningpoint.RotatedBy(Projectile.rotation - (float)Math.PI / 2f);
                            int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, DustID.TerraBlade, 0f, 0f, 160);
                            Main.dust[dustIndex].scale = 1.1f;
                            Main.dust[dustIndex].noGravity = true;
                            Main.dust[dustIndex].position = Projectile.Center + spinningpoint;
                            Main.dust[dustIndex].velocity = Projectile.velocity * 0.1f;
                            Main.dust[dustIndex].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[dustIndex].position) * 1.25f;
                        }
                    }
                    Projectile.ai[0] = 2;
                }
            }
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            var source = Projectile.GetSource_FromThis();

            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 60);

            if (Projectile.ai[0] == 0)
			{
                int projIn = Projectile.NewProjectile(source, Projectile.Center, Vector2.Normalize(Projectile.velocity).RotatedByRandom(MathHelper.ToRadians(10)), ModContent.ProjectileType<ExobeamSlash>(), Projectile.damage / 2, 0, Main.LocalPlayer.whoAmI);
                Main.projectile[projIn].DamageType = DamageClass.Ranged;
                Main.projectile[projIn].scale = 0.4f;
                Main.projectile[projIn].penetrate = 1;
                Main.projectile[projIn].CritChance = 0;
                if (hit.Crit)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int projIn2 = Projectile.NewProjectile(source, new Vector2(Projectile.Center.X + Main.rand.Next(-32, 33), Projectile.Center.Y + Main.rand.Next(-128, -95)), new Vector2(Main.rand.Next(-15, 16), 6), ModContent.ProjectileType<RainbowBlast>(), Projectile.damage / 5, 0, Main.LocalPlayer.whoAmI);
                        Main.projectile[projIn2].tileCollide = false;
                        Main.projectile[projIn2].CritChance = 0;
                    }
                }
            }
			else
			{
                if (hit.Crit)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int projIn2 = Projectile.NewProjectile(source, new Vector2(Projectile.Center.X + Main.rand.Next(-32, 33), Projectile.Center.Y + Main.rand.Next(-128, -95)), new Vector2(Main.rand.Next(-15, 16), 6), ModContent.ProjectileType<RainbowBlast>(), Projectile.damage / 5, 0, Main.LocalPlayer.whoAmI);
                        Main.projectile[projIn2].tileCollide = false;
                        Main.projectile[projIn2].CritChance = 0;
                    }
                }
                int projIn3 = Projectile.NewProjectile(source, Projectile.Center, new Vector2(0, 0), ModContent.ProjectileType<PrismExplosionSmall>(), Projectile.damage / 4, 0, Main.LocalPlayer.whoAmI);
                Main.projectile[projIn3].localNPCHitCooldown = -1;
                Projectile.Kill();
            }
        }
		public override bool PreDraw(ref Color lightColor) 
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(new Color(255, 255, 255, 255));
            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
		}
    }
}
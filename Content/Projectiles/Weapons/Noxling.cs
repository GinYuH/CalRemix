using CalamityMod;
using CalamityMod.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class Noxling : ModProjectile
    {
        public float State;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults() 
        {
            Projectile.width = 36;
			Projectile.height = 36;
			Projectile.aiStyle = ProjAIStyleID.FallingStar;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.penetrate = 5;
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(State);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            State = reader.ReadSingle();
        }
        public override void AI()
        {
            Projectile.alpha = 0;
            if (State >= 1)
            {
                Projectile.ChargingMinionAI(800f, 1500f, 2400f, 150f, 0, 30f, 30f, 15f, new Vector2(0f, 0f), 30f, 12f, tileVision: true, ignoreTilesWhenCharging: true);
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 180);
            if (State == 0)
            {
                Projectile.velocity.RotatedByRandom(MathHelper.TwoPi);
                Projectile.aiStyle = -1;
                Projectile.timeLeft = 600;
                State = 1;
            }
            else if (State >= 4)
                Projectile.Kill();
            else
                State++;
        }
        public override void OnKill(int timeLeft)
        {
            if (State >= 4)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaGrenadeSmallExplosion>(), Projectile.damage, Projectile.knockBack);
                proj.DamageType = DamageClass.Magic;
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    Color newColor = Main.hslToRgb(1f, 1f, 0.5f);
                    int index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 0, newColor);
                    Main.dust[index].position = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height);
                    Main.dust[index].velocity *= Main.rand.NextFloat() * 2.4f;
                    Main.dust[index].noGravity = true;
                    Main.dust[index].fadeIn = 0.6f + Main.rand.NextFloat();
                    Main.dust[index].scale = 1.4f;
                    if (index != 6000)
                    {
                        Dust dust = Dust.CloneDust(index);
                        dust.scale /= 2f;
                        dust.fadeIn *= 0.85f;
                        dust.color = new Color(255, 255, 255, 255);
                    }
                }
            }
        }
    }
}
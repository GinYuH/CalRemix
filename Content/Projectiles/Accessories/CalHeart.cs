using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Projectiles.Damageable;

namespace CalRemix.Content.Projectiles.Accessories
{
    public class CalHeart : DamageableProjectile
    {
        public override string Texture => "CalamityMod/NPCs/SupremeCalamitas/BrimstoneHeart";
        public override int LifeMax => 66666;
        public override int MaxDamageImmunityFrames => 5;
        public override DamageSourceType DamageSources => DamageSourceType.HostileProjectiles;
        public override SoundStyle HitSound => SoundID.NPCHit13;
        public override SoundStyle DeathSound => SoundID.NPCDeath1;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Calamitous Heart");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
        }

        public override void SafeAI()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player pla = Main.player[i];
                if (pla.getRect().Intersects(Projectile.getRect()))
                {
                    pla.Heal(10);
                    Projectile.active = false;
                }
            }
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] >= 600)
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 pos = new Vector2(Projectile.Center.X + 10, Projectile.Center.Y + 10);
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<CalamityMod.Projectiles.Summon.BrimstoneExplosionMinion>(), 6666, 0, Projectile.owner);
                    if (p.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[p].originalDamage = 6666;
                        Main.projectile[p].scale *= 3;
                    }
                }
                Projectile.active = false;
            }
            int framra = 12 - (int)(Projectile.localAI[0] / 50);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > framra)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 5)
                Projectile.frame = 0;

            if (Projectile.penetrate == 0)
            {
                Projectile.Kill();
            }
        }

        public override void HitEffectProjectile(int damage, Projectile target)
        {
            Projectile.penetrate--;
            target.active = false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
                Vector2 pos = new Vector2(Projectile.Center.X + 10, Projectile.Center.Y + 10);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<CalamityMod.Projectiles.Summon.BrimstoneExplosionMinion>(), 6666, 0, Projectile.owner);
                if (p.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[p].originalDamage = 6666;
                }
            }
        }
    }
}

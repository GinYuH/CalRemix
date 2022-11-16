using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Projectiles;
using CalamityMod.Projectiles.Damageable;

namespace CalRemix.Projectiles
{
    public class LumChunk : DamageableProjectile
    {
        public override string Texture => "CalRemix/Items/Weapons/EcologicalCollapse";
        public override int LifeMax => 33000;
        public override int MaxDamageImmunityFrames => 5;
        public override DamageSourceType DamageSources => DamageSourceType.HostileProjectiles;
        public override SoundStyle HitSound => SoundID.NPCHit13;
        public override SoundStyle DeathSound => SoundID.NPCDeath1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lumenyl Shard");
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

        public override void SafeAI()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player pla = Main.player[i];
                if (pla.getRect().Intersects(Projectile.getRect()))
                {
                    pla.Heal(100);
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
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<CalamityMod.Projectiles.Summon.BrimstoneExplosionMinion>(), 100000, 0, Projectile.owner);
                    if (p.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[p].originalDamage = 100000;
                        Main.projectile[p].scale *= 3;
                    }
                }
                Projectile.active = false;
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC tar = Main.npc[i];
                if (tar.damage > 0 && !tar.friendly && !tar.dontTakeDamage && tar.chaseable && !tar.boss)
                {
                    Vector2 direction = Projectile.Center - tar.Center;
                    direction.Normalize();
                    tar.velocity = direction * 4;
                    tar.noTileCollide = true;
                }
            }
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile tar = Main.projectile[i];
                if (tar.damage > 0 && tar.damage < 400 && !tar.friendly)
                {
                    Vector2 direction = Projectile.Center - tar.Center;
                    direction.Normalize();
                    tar.velocity = direction * 4;
                    tar.tileCollide = false;
                }
            }

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

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
                Vector2 pos = new Vector2(Projectile.Center.X + 10, Projectile.Center.Y + 10);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<CalamityMod.Projectiles.Summon.BrimstoneExplosionMinion>(), 100000, 0, Projectile.owner);
                if (p.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[p].originalDamage = 100000;
                }
            }
        }
    }
}
